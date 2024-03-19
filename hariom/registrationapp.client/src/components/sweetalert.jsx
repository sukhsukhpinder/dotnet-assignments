import Swal from 'sweetalert2';

const showAlert = (title, text, icon) => {
    Swal.fire({
        title: title,
        text: text,
        icon: icon,
        timer: 3000, // Automatically close the alert after 3 seconds
        timerProgressBar: true,
        showConfirmButton: false
    });
};

export default showAlert;