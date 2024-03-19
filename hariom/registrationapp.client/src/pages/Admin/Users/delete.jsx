import PropTypes from 'prop-types';
import {
    MDBBtn,
    MDBModal,
    MDBModalDialog,
    MDBModalContent,
    MDBModalHeader,
    MDBModalTitle,
    MDBModalBody,
    MDBModalFooter
} from 'mdb-react-ui-kit';
import showAlert from '../../../components/sweetalert';
import Endpoints from '../../../components/endpoints';

function DeleteModal({ isOpen, toggleOpen, user }) {
    const handleSubmit = async () => {
        try {
            const response = await fetch(`${Endpoints.Users}/${user.id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('token')}`
                }
            });

            if (response.ok) {
                console.log('Deleted successful');
                showAlert('Success', 'Deleted successful', 'success');           
            } else {
                let responseMsg = await response.text();
                console.error('Delete failed:', responseMsg);
                showAlert('Error', 'Delete failed:' + responseMsg, 'error');                   
            }
        } catch (error) {
            console.error('Delete failed:', error.message);
            showAlert('Error', 'Delete failed:' + error.message, 'error');   
        }
    };

    const handleConfirmDelete = () => {
        handleSubmit()
            .then(() => {
                toggleOpen();
            })
            .catch((error) => {
                console.error('Error during deletion:', error);
            });
    };

    const handleClose = () => {
        toggleOpen(); // Close the modal without deleting
    };

    return (
        <MDBModal open={isOpen} setOpen={() => { }} tabIndex='-1'>
            <MDBModalDialog>
                <MDBModalContent>
                    <MDBModalHeader>
                        <MDBModalTitle>Delete Confirmation</MDBModalTitle>
                        <MDBBtn className='btn-close' color='none' onClick={handleClose}></MDBBtn>
                    </MDBModalHeader>
                    <MDBModalBody>
                        Are you sure you want to delete this item?
                    </MDBModalBody>
                    <MDBModalFooter>
                        <MDBBtn color='secondary' onClick={handleClose}>
                            Cancel
                        </MDBBtn>
                        <MDBBtn color='danger' onClick={handleConfirmDelete}>
                            Delete
                        </MDBBtn>
                    </MDBModalFooter>
                </MDBModalContent>
            </MDBModalDialog>
        </MDBModal>
    );
}

DeleteModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    toggleOpen: PropTypes.func.isRequired,
    user: PropTypes.object,
};

export default DeleteModal;
