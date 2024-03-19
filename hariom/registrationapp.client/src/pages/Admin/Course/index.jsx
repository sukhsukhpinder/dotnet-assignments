import Header from '../../../components/header';
import { useState, useEffect } from 'react';
import Endpoints from '../../../components/endpoints';
import AddCourse from '../Course/add';
import EditCourse from '../Course/edit';
import DeleteCourse from '../Course/delete';
import { MDBBtn, MDBTable, MDBTableHead, MDBTableBody } from 'mdb-react-ui-kit';
export default function Courses() {
    const [courses, setCourses] = useState([]);
    const [editCourseModal, setEditCourseModal] = useState(false);
    const [addCourseModal, setAddCourseModal] = useState(false);

    const getCourses = () => {
        fetch(Endpoints.Courses, {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            }
        })
            .then(response => response?.json())
            .then(data => {
                // Update state with fetched data
                setCourses(data.result);
            })
            .catch(error => {
                console.error('Error fetching data:', error);
                alert('Error fetching data:' + error);
            });
    }

    const toggleAddCourseOpen = () => {
        setAddCourseModal(prevState => !prevState);
        getCourses();
    };
    const toggleEditCourseOpen = (selectedUser) => {
        setSelectedCourse(selectedUser);
        setEditCourseModal(prevState => !prevState);
        getCourses();
    };
    const [deleteUserModal, setDeleteUserModal] = useState(false);
    const toggleDeleteCourseOpen = (selectedUser) => {
        setSelectedCourse(selectedUser);
        setDeleteUserModal(prevState => !prevState);
        getCourses();
    };
    const [selectedCourse, setSelectedCourse] = useState(null);
    useEffect(() => {
        // Fetch data from the endpoint
        getCourses();
    }, []);

    return (
        <div>
            <Header />
            <div className="float-end mt-3">
                <MDBBtn color='primary' onClick={() => toggleAddCourseOpen()}>
                    Add Course
                </MDBBtn>
            </div>
            <MDBTable align='middle'>
                <MDBTableHead>
                    <tr>
                        <th scope='col'>Name</th>
                        <th scope='col'>Description</th>
                        <th scope='col'>Coast</th>
                        <th scope='col'>DurationInMonths</th>
                        <th scope='col'>Actions</th>
                    </tr>
                </MDBTableHead>
                <MDBTableBody>
                    {/* Map through registrations and render each row */}
                    {courses.map(course => (
                        <tr key={course?.id}>
                            <td>
                                <p className='fw-normal mb-1'>{course.name}</p>
                            </td>
                            <td>
                                <p className='fw-normal mb-1'>{course.description}</p>
                            </td>
                            <td>
                                <p className='fw-normal mb-1'>{course.cost}</p>
                            </td>
                            <td>
                                <p className='fw-normal mb-1'>{course.durationInMonths}</p>
                            </td>

                            <td>
                                <MDBBtn color='link' rounded size='sm' onClick={() => toggleEditCourseOpen(course)}>
                                    Edit
                                </MDBBtn>
                                <MDBBtn color='link' rounded size='sm' onClick={() => toggleDeleteCourseOpen(course)}>
                                    Delete
                                </MDBBtn>
                            </td>
                        </tr>
                    ))}
                </MDBTableBody>
            </MDBTable>
            <AddCourse isOpen={addCourseModal} toggleOpen={toggleAddCourseOpen} />
            <EditCourse isOpen={editCourseModal} toggleOpen={toggleEditCourseOpen} course={selectedCourse} />
            <DeleteCourse isOpen={deleteUserModal} toggleOpen={toggleDeleteCourseOpen} course={selectedCourse} />
        </div>
    );
}
