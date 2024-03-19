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
    MDBIcon
} from 'mdb-react-ui-kit';

const Auth = () => {
    const [formData, setFormData] = useState({
        Email: '',
        Password: '',
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const [/*token*/, setToken] = useState('');


    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch(Endpoints.Token, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });
            if (!response.ok) {
                console.error('Login failed');
                alert('Login failed' + await response.text())
            } else {
                const data = await response.json();
                if (response.ok) {
                    const token = data.result.token;
                    setToken(token);
                    localStorage.setItem('token', token);
                    window.location.href = "/users";
                }
            }
        } catch (error) {
            console.error('Login failed', error);
            alert('Login failed' + error)
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
                                <form onSubmit={handleLogin}>
                                    <div className="d-flex flex-row align-items-center mb-4">
                                        <MDBIcon fas icon="envelope me-3" size='lg' />
                                        <MDBInput label='Your Email' id='email' name='Email' type='email' value={formData.Email} onChange={handleChange} required />
                                    </div>
                                    <div className="d-flex flex-row align-items-center mb-4">
                                        <MDBIcon fas icon="lock me-3" size='lg' />
                                        <MDBInput label='Password' id='password' name='Password' type='password' value={formData.Password} onChange={handleChange} required />
                                    </div>
                                    <MDBBtn className='mb-4' size='lg' type='submit'>Login</MDBBtn>
                                    <MDBBtn className='mb-4' size='lg' color='link' href='/registration' rippleColor='dark'>
                                        Register Now
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

export default Auth;
