import { useState, useEffect } from 'react';
import './App.css';
import Registration from './pages/registration';
import WheatherForecast from './pages/weatherforecast';
import Login from './pages/login';
import Users from './pages/Admin/Users/index';
import Courses from './pages/Admin/Course/index';
import Protected from './components/protected';

import { Routes, Route, BrowserRouter } from 'react-router-dom';

function App() {


    const isToken = localStorage.getItem('token');
    const [isSignedIn, setIsSignedIn] = useState(isToken !== null);

    useEffect(() => {
        if (isToken) {
            setIsSignedIn(true)
        } else {
            setIsSignedIn(false)
        }
    }, [])

    return (

        <div className='wrapper'>
            <BrowserRouter>
                <Routes>
                    <Route path="/" exact element={<Login />} />
                    <Route path="/registration" exact
                        element={
                            <Registration />
                        } />
                    <Route path="/weatherforecast" exact
                        element={
                            <WheatherForecast />
                        } />
                    <Route path="/users" exact
                        element={
                            <Protected isSignedIn={isSignedIn}>
                                <Users />
                            </Protected>
                        } />
                    <Route path="/courses" exact
                        element={
                            <Protected isSignedIn={isSignedIn}>
                                <Courses />
                            </Protected>
                        } />
                </Routes>
            </BrowserRouter>
        </div>
    );
}
export default App;