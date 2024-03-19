import { Navigate } from 'react-router-dom';
import PropTypes from 'prop-types';

function Protected({ isSignedIn, children }) {
    if (!isSignedIn) {
        return <Navigate to="/" />;
    }
    return children;
}

Protected.propTypes = {
    isSignedIn: PropTypes.bool.isRequired, // Add prop type validation for isSignedIn
    children: PropTypes.node.isRequired, // Assuming children is a React node
};

export default Protected;
