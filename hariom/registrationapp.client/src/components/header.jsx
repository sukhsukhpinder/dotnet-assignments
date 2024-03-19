import { useState } from 'react';
import {
    MDBContainer,
    MDBNavbar,
    MDBNavbarBrand,
    MDBNavbarToggler,
    MDBIcon,
    MDBNavbarNav,
    MDBNavbarItem,
    MDBNavbarLink,
    MDBBtn,
    //MDBDropdown,
    //MDBDropdownToggle,
    //MDBDropdownMenu,
    //MDBDropdownItem,
    MDBCollapse
} from 'mdb-react-ui-kit';

export default function App() {
    const [openBasic, setOpenBasic] = useState(false);
    const handleLogout = () => {
        localStorage.removeItem('token');
        window.location.href = "/";
    };

    const token = localStorage.getItem('token');

    return (
        <MDBNavbar expand='lg' light bgColor='light'>
            <MDBContainer fluid>
                <MDBNavbarBrand href='#'>Brand</MDBNavbarBrand>

                <MDBNavbarToggler
                    aria-controls='navbarSupportedContent'
                    aria-expanded='false'
                    aria-label='Toggle navigation'
                    onClick={() => setOpenBasic(!openBasic)}
                >
                    <MDBIcon icon='bars' fas />
                </MDBNavbarToggler>

                <MDBCollapse navbar open={openBasic}>
                    <MDBNavbarNav className='mr-auto mb-2 mb-lg-0'>
                        {token && token !== "" && token !== "undefined" && (
                            <><MDBNavbarItem>
                                <MDBNavbarLink active aria-current='page' href='/users'>
                                    Users
                                </MDBNavbarLink>
                            </MDBNavbarItem><MDBNavbarItem>
                                    <MDBNavbarLink active aria-current='page' href='/courses'>
                                        Courses
                                    </MDBNavbarLink>
                                </MDBNavbarItem></>
                        )}
                        {/*<MDBNavbarItem>*/}
                        {/*    <MDBNavbarLink href='#'>Link</MDBNavbarLink>*/}
                        {/*</MDBNavbarItem>*/}

                        {/*<MDBNavbarItem>*/}
                        {/*    <MDBDropdown>*/}
                        {/*        <MDBDropdownToggle tag='a' className='nav-link' role='button'>*/}
                        {/*            Dropdown*/}
                        {/*        </MDBDropdownToggle>*/}
                        {/*        <MDBDropdownMenu>*/}
                        {/*            <MDBDropdownItem link>Action</MDBDropdownItem>*/}
                        {/*            <MDBDropdownItem link>Another action</MDBDropdownItem>*/}
                        {/*            <MDBDropdownItem link>Something else here</MDBDropdownItem>*/}
                        {/*        </MDBDropdownMenu>*/}
                        {/*    </MDBDropdown>*/}
                        {/*</MDBNavbarItem>*/}

                        {/*<MDBNavbarItem>*/}
                        {/*    <MDBNavbarLink disabled href='#' tabIndex={-1} aria-disabled='true'>*/}
                        {/*        Disabled*/}
                        {/*    </MDBNavbarLink>*/}
                        {/*</MDBNavbarItem>*/}

                    </MDBNavbarNav>

                    <form className='d-flex input-group w-auto'>
                        <input type='search' className='form-control' placeholder='Type query' aria-label='Search' />
                        <MDBBtn color='primary'>Search</MDBBtn>
                    </form>

                    {token && token !== "" && token !== "undefined" && ( // Condition to check if token exists and is not empty
                        <ul className='navbar-nav w-80'>
                            <MDBNavbarItem>
                                <MDBNavbarLink onClick={handleLogout} tabIndex={-1}>
                                    Logout
                                </MDBNavbarLink>
                            </MDBNavbarItem>
                        </ul>
                    )}

                </MDBCollapse>
            </MDBContainer>
        </MDBNavbar>
    );
}