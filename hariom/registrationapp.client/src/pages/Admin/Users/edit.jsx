import { useState, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import Endpoints from '../../../components/endpoints';
import showAlert from '../../../components/sweetalert';
import { MDBBtn, MDBModal, MDBModalDialog, MDBModalContent, MDBModalHeader, MDBModalTitle, MDBModalBody, MDBModalFooter, MDBIcon, MDBInput } from 'mdb-react-ui-kit';

function EditUserModal({ isOpen, toggleOpen, user }) {
    const parseDOB = (dob) => {
        if (!dob) return '';
        const parts = dob.split('/');
        const day = parts[0];
        const month = parts[1];
        const year = parts[2];
        const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        const monthNumber = monthNames.findIndex(name => name === month) + 1;
        const formattedDate = `${year}-${monthNumber.toString().padStart(2, '0')}-${day.padStart(2, '0')}`;
        return formattedDate;
    };

    const [currentProfile, setCurrentProfile] = useState({
        name: user?.name || '',
        dob: parseDOB(user?.dob) || '',
        email: user?.email || '',
        password: ''
    });

    const [updatedProfile, setUpdatedProfile] = useState({
        name: '',
        dob: '',
        email: '',
        password: ''
    });

    const nameRef = useRef(null);
    const dobRef = useRef(null);
    const emailRef = useRef(null);
    const passwordRef = useRef(null);

    const handleChange = () => {
        setUpdatedProfile({
            name: nameRef.current.value,
            dob: dobRef.current.value,
            email: emailRef.current.value,
            password: passwordRef.current.value
        });
    };

    const handleClose = () => {
        toggleOpen();
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch(`${Endpoints.Users}/${user.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                //body: JSON.stringify(updatedProfile)
                body: JSON.stringify({
                    name: updatedProfile.name,
                    email: updatedProfile.email,
                    password: updatedProfile.password,
                    dob: updatedProfile.dob
                })
            });

            if (response.ok) {
                console.log('Updated successful');
                showAlert('Success', 'Updated successful', 'success');     
                toggleOpen();
            } else {
                let responseMsg = await response.text();
                console.error('Update failed:', responseMsg);
                showAlert('Error', 'Update failed:' + responseMsg, 'error');                  
            }
        } catch (error) {
            console.error('Update failed:', error.message);
            showAlert('Error', 'Update failed:' + error.message, 'error');              
        }
    };

    useEffect(() => {
        setCurrentProfile({
            name: user?.name || '',
            dob: parseDOB(user?.dob) || '',
            email: user?.email || '',
            password: ''
        });
    }, [user]);

    useEffect(() => {
        if (nameRef.current) {
            nameRef.current.value = currentProfile.name || "";
        }
        if (dobRef.current) {
            dobRef.current.value = currentProfile.dob || "";
        }
        if (emailRef.current) {
            emailRef.current.value = currentProfile.email || "";
        }
        if (passwordRef.current) {
            passwordRef.current.value = currentProfile.password || "";
        }
    }, [
        currentProfile.name,
        currentProfile.dob,
        currentProfile.email,
        currentProfile.password
    ]);

    return (
        <MDBModal open={isOpen} setOpen={() => { }} tabIndex='-1'>
            <form onSubmit={handleSubmit}>
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBModalTitle>Modal title</MDBModalTitle>
                            <MDBBtn className='btn-close' color='none' onClick={handleClose}></MDBBtn>
                        </MDBModalHeader>
                        <MDBModalBody>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg' />
                                <MDBInput label='Your Name' id='name' name='Name' type='text' className='w-100 active' ref={nameRef} onChange={handleChange} required />                                
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="calendar-alt me-3" size='lg' />
                                <MDBInput label='Your DOB' id='dob' name='dob' type='date' className='w-100 active' ref={dobRef} onChange={handleChange} required />                                
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="envelope me-3" size='lg' />
                                <MDBInput label='Your Email' id='email' name='email' type='email' className='w-100 active' ref={emailRef} onChange={handleChange} required />                                
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="lock me-3" size='lg' />
                                <MDBInput label='Password' id='password' name='password' type='password' className='w-100 active' ref={passwordRef} onChange={handleChange} required />                                
                            </div>
                        </MDBModalBody>
                        <MDBModalFooter>
                            <MDBBtn color='secondary' onClick={handleClose}>
                                Close
                            </MDBBtn>
                            <MDBBtn className='primary' type="submit">Save changes</MDBBtn>
                        </MDBModalFooter>
                    </MDBModalContent>
                </MDBModalDialog>
            </form>
        </MDBModal>
    );
}

// Define propTypes
EditUserModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    toggleOpen: PropTypes.func.isRequired,
    user: PropTypes.object,
};

export default EditUserModal;