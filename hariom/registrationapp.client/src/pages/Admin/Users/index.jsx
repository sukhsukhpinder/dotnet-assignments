import Header from '../../../components/header';
import { useState, useEffect } from 'react';
import Endpoints from '../../../components/endpoints';
import EditUser from './edit';
import DeleteUser from './delete';
import { MDBBadge, MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit';


export default function Dashboard() {
    const [users, setUsers] = useState([]);
    const [editUserModal, setEditUserModal] = useState(false);

    const getUsers = () => {
        fetch(Endpoints.Users, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(response => response.json())
            .then(data => {
                // Update state with fetched data
                setUsers(data.result);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
                alert('Error fetching data:' + error);
            });
    }


    const toggleEditUserOpen = (selectedUser) => {
        setSelectedUser(selectedUser);
        setEditUserModal(prevState => !prevState);
        getUsers();
    };
    const [deleteUserModal, setDeleteUserModal] = useState(false);
    const toggleDeleteUserOpen = (selectedUser) => {
        setSelectedUser(selectedUser);
        setDeleteUserModal(prevState => !prevState);
        getUsers();
    };
    const [selectedUser, setSelectedUser] = useState(null);
    useEffect(() => {
        // Fetch data from the endpoint
        getUsers();
    }, []);

    return (
        <div>
            <Header />
            <MDBTable align='middle'>
                <MDBTableHead>
                    <tr>
                        <th scope='col'>Name</th>
                        <th scope='col'>Type</th>
                        <th scope='col'>Date-Of-Birth</th>
                        <th scope='col'>Actions</th>
                    </tr>
                </MDBTableHead>
                <MDBTableBody>
                    {/* Map through registrations and render each row */}
                    {users.map(user => (
                        <tr key={user.id}>
                            <td>
                                <div className='d-flex align-items-center'>
                                    <img
                                        src='https://mdbootstrap.com/img/new/avatars/6.jpg'
                                        alt=''
                                        style={{ width: '45px', height: '45px' }}
                                        className='rounded-circle'
                                    />
                                    <div className='ms-5 ps-5'>
                                        <p className='fw-bold mb-1 ms-5 ps-5'>{user.name}</p>
                                        <p className='text-muted mb-0 ms-5 ps-5'>{user.email}</p>
                                    </div>
                                </div>
                            </td>
                            {/*<td>*/}
                            {/*    <p className='fw-normal mb-1'>{registration.title}</p>*/}
                            {/*    <p className='text-muted mb-0'>{registration.department}</p>*/}
                            {/*</td>*/}
                            <td>
                                <MDBBadge color={user.role.toLowerCase() === 'admin' ? 'success' : user.role.toLowerCase() === 'student' ? 'primary' : 'warning'} pill>
                                    {user.role}
                                </MDBBadge>
                            </td>
                            <td>{user.dob}</td>
                            <td>
                                <MDBBtn color='link' rounded size='sm' onClick={() => toggleEditUserOpen(user)}>
                                    Edit
                                </MDBBtn>
                                <MDBBtn color='link' rounded size='sm' onClick={() => toggleDeleteUserOpen(user)}>
                                    Delete
                                </MDBBtn>
                            </td>
                        </tr>
                    ))}
                </MDBTableBody>
            </MDBTable>
            <EditUser isOpen={editUserModal} toggleOpen={toggleEditUserOpen} user={selectedUser} />
            <DeleteUser isOpen={deleteUserModal} toggleOpen={toggleDeleteUserOpen} user={selectedUser} />
        </div>
    );
}
