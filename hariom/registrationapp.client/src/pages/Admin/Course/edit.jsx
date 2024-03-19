import { useState, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import Endpoints from '../../../components/endpoints';
import showAlert from '../../../components/sweetalert';
import { MDBBtn, MDBModal, MDBModalDialog, MDBModalContent, MDBModalHeader, MDBModalTitle, MDBModalBody, MDBModalFooter, MDBIcon, MDBInput } from 'mdb-react-ui-kit';

function EditCourseModal({ isOpen, toggleOpen, course }) {

    const [currentProfile, setCurrentProfile] = useState({
        name: course?.name || '',
        cost: course?.cost || '',
        description: course?.description || '',
        durationInMonths: course?.durationInMonths || '',
    });

    const [updatedProfile, setUpdatedProfile] = useState({
        name: '',
        cost: '',
        description: '',
        durationInMonths: ''
    });

    const nameRef = useRef(null);
    const costRef = useRef(null);
    const descriptionRef = useRef(null);
    const durationInMonthsRef = useRef(null);

    const handleChange = () => {
        setUpdatedProfile({
            name: nameRef.current.value,
            cost: costRef.current.value,
            description: descriptionRef.current.value,
            durationInMonths: durationInMonthsRef.current.value
        });
    };

    const handleClose = (e) => {
        e.preventDefault();
        toggleOpen();
    };

    const handleSubmit = async (e) => {
        e.preventDefault(); // Prevent default form submission behavior
        try {
            const response = await fetch(`${Endpoints.Courses}/${course?.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify(updatedProfile)
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
            name: course?.name || '',
            cost: course?.cost || '',
            description: course?.description || '',
            durationInMonths: course?.durationInMonths || '',
        });
    }, [course]);

    useEffect(() => {
        if (nameRef.current) {
            nameRef.current.value = currentProfile.name || "";
        }
        if (costRef.current) {
            costRef.current.value = currentProfile.cost || "";
        }
        if (descriptionRef.current) {
            descriptionRef.current.value = currentProfile.description || "";
        }
        if (durationInMonthsRef.current) {
            durationInMonthsRef.current.value = currentProfile.durationInMonths || "";
        }
    }, [
        currentProfile.name,
        currentProfile.cost,
        currentProfile.description,
        currentProfile.durationInMonths
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
                                <MDBInput label='Course Name' id='name' name='Name' type='text' className='w-100 active' ref={nameRef} onChange={handleChange} required />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="calendar-alt me-3" size='lg' />
                                <MDBInput label='Course Description' id='description' name='description' type='textarea' className='w-100 active' ref={descriptionRef} onChange={handleChange} required />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="envelope me-3" size='lg' />
                                <MDBInput label='Course Cost' id='cost' name='cost' type='number' className='w-100 active' ref={costRef} onChange={handleChange} required />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4">
                                <MDBIcon fas icon="lock me-3" size='lg' />
                                <MDBInput label='Duration In Months' id='durationInMonths' name='durationInMonths' type='number' className='w-100 active' ref={durationInMonthsRef} onChange={handleChange} required />
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
EditCourseModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    toggleOpen: PropTypes.func.isRequired,
    course: PropTypes.object,
};

export default EditCourseModal;
