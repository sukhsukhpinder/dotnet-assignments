import { useState } from 'react';
import PropTypes from 'prop-types';
import Endpoints from '../../../components/endpoints';
import showAlert from '../../../components/sweetalert';
import {
    MDBBtn,
    MDBModal,
    MDBModalDialog,
    MDBModalContent,
    MDBModalHeader,
    MDBModalTitle,
    MDBModalBody,
    MDBModalFooter,
    MDBIcon,
    MDBInput
} from 'mdb-react-ui-kit';

function AddCourseModal({ isOpen, toggleOpen }) {//onAddCourse

    const initialCourseData = {
        name: '',
        description: '',
        cost: '',
        durationInMonths: ''
    };
    const [courseData, setCourseData] = useState(initialCourseData);

    const handleClose = () => {
        setCourseData(initialCourseData);
        toggleOpen(); // Close the modal without adding a course
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch(Endpoints.Courses, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify(courseData),
            });

            if (response.ok) {
                console.log('Course added successfully');
                showAlert('Success', 'Course added successfully', 'success');   
                setCourseData(initialCourseData);
                //onAddCourse(); // Notify parent component about the addition of a new course
                toggleOpen(); // Close the modal after adding the course
            } else {
                let responseMsg = await response.text();
                console.error('Failed to add course:', responseMsg);
                showAlert('Error', 'Failed to add course: ' + responseMsg, 'error');
            }
        } catch (error) {
            console.error('Failed to add course:', error.message);
            showAlert('Error', 'Failed to add course: ' + error.message, 'error');
        }
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setCourseData({ ...courseData, [name]: value });
    };

    return (
        <MDBModal open={isOpen} setOpen={() => { }} tabIndex='-1'>
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Add Course</MDBModalTitle>
                        <MDBBtn className='btn-close' color='none' onClick={handleClose}></MDBBtn>
                    </MDBModalHeader>
                    <MDBModalBody>
                        <form onSubmit={handleSubmit}>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg' />
                                <MDBInput label='Course Name' id='name' name='name' type='text' className='w-100' value={courseData.name} onChange={handleChange} required />                                                                
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg' />
                                <MDBInput label='Course Description' id='description' name='description' type='textarea' className='w-100' value={courseData.description} onChange={handleChange} required />
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg' />
                                <MDBInput label='Course Cost' id='cost' name='cost' type='number' className='w-100' value={courseData.cost} onChange={handleChange} required />                                
                            </div>
                            <div className="d-flex flex-row align-items-center mb-4 ">
                                <MDBIcon fas icon="user me-3" size='lg' />
                                <MDBInput label='Duration In Months' id='durationInMonths' name='durationInMonths' type='number' className='w-100' value={courseData.durationInMonths} onChange={handleChange} required />                                
                            </div>
                        </form>
                    </MDBModalBody>
                    <MDBModalFooter>
                        <MDBBtn color='secondary' onClick={handleClose}>
                            Cancel
                        </MDBBtn>
                        <MDBBtn color='primary' onClick={handleSubmit}>
                            Add Course
                        </MDBBtn>
                    </MDBModalFooter>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
    );
}

AddCourseModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    toggleOpen: PropTypes.func.isRequired,
    /*onAddCourse: PropTypes.func.isRequired,*/
};

export default AddCourseModal;
