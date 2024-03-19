import { useState } from 'react';
import Header from '.././components/header';
import Endpoints from '../components/endpoints';
import {
    MDBBtn,
    MDBContainer,
    MDBRow,
    MDBCol,
    MDBCard,
    MDBCardBody,
    MDBCardImage,
    MDBInput,
    MDBIcon,
} from 'mdb-react-ui-kit';

function App() {
    const [formData, setFormData] = useState({
        Name: '',
        Email: '',
        Password: '',
        DOB:''
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch(Endpoints.Users, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });
            if (response.ok) {
                console.log('Registration successful');
                alert('Registration successful');
                window.location.href = "/";
                // You can redirect the user to another page or show a success message here
            } else {
                let responseMsg = await response.text();
                console.error('Registration failed:', responseMsg);
                // You can display an error message to the user here
                alert('Registration failed:' + responseMsg);
            }
        } catch (error) {
            console.error('Registration failed:', error.message);
            // You can display an error message to the user here
            alert('Registration failed:' + await error.message);
        }
    };

    return (
        <div>
            <Header></Header>
            <MDBContainer fluid>
                <MDBCard className='text-black m-5' style={{ borderRadius: '25px' }}>
                    <MDBCardBody>
                        <MDBRow>
                            <MDBCol md='10' lg='6' className='order-2 order-lg-1 d-flex flex-column align-items-center'>
                                <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
                                <form onSubmit={handleSubmit}>
                                    <div className="d-flex flex-row align-items-center mb-4 ">
                                        <MDBIcon fas icon="user me-3" size='lg' />
                                        <MDBInput label='Your Name' id='name' name='Name' type='text' className='w-100' value={formData.Name} onChange={handleChange} required />
                                    </div>
                                    <div className="d-flex flex-row align-items-center mb-4 ">
                                        <MDBIcon fas icon="user me-3" size='lg' />
                                        <MDBInput label='Your DOB' id='dob' name='DOB' type='date' className='w-100' value={formData.DOB} onChange={handleChange} required />
                                    </div>
                                    <div className="d-flex flex-row align-items-center mb-4">
                                        <MDBIcon fas icon="envelope me-3" size='lg' />
                                        <MDBInput label='Your Email' id='email' name='Email' type='email' value={formData.Email} onChange={handleChange} required />
                                    </div>
                                    <div className="d-flex flex-row align-items-center mb-4">
                                        <MDBIcon fas icon="lock me-3" size='lg' />
                                        <MDBInput label='Password' id='password' name='Password' type='password' value={formData.Password} onChange={handleChange} required />
                                    </div>
                                    <MDBBtn className='mb-4' size='lg' type='submit'>Register</MDBBtn>
                                    <MDBBtn className='mb-4' size='lg' color='link' href='/' rippleColor='dark'>
                                        Login
                                    </MDBBtn>
                                </form>

                            </MDBCol>
                            <MDBCol md='10' lg='6' className='order-1 order-lg-2 d-flex align-items-center'>
                                <MDBCardImage src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp' fluid />
                            </MDBCol>
                        </MDBRow>
                    </MDBCardBody>
                </MDBCard>
            </MDBContainer>
        </div>
    );
}

export default App;
