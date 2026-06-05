import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { Container, Card, Form, Button } from 'react-bootstrap';
import logoImg from '../assets/logo.png';
import textLogoImg from '../assets/text_logo.jpg';

const Login = () => {
    const [phone, setPhone] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [clientName, setClientName] = useState('');
    const [isRegistering, setIsRegistering] = useState(false);
    const [error, setError] = useState('');
    const { login, register } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            if (isRegistering) {
                await register({
                    clientName: clientName,
                    phoneNumber: phone,
                    email: email,
                    password: password,
                    role: 0
                });
                const user = await login(phone, password);
                handleRedirect(user);
            } else {
                const user = await login(phone, password);
                handleRedirect(user);
            }
        } catch (err) {
            setError(err.response?.data?.message || 'Authentication failed');
        }
    };

    const handleRedirect = (user) => {
        if (user.role === 'Admin') navigate('/admin');
        else if (user.role === 'Manager') navigate('/manager');
        else navigate('/map');
    };

    return (
        <Container className="d-flex justify-content-center align-items-center min-vh-100">
            <Card className="shadow-lg border-danger" style={{ width: '100%', maxWidth: '400px', borderRadius: '12px' }}>
                <Card.Header className="bg-white border-bottom border-warning pt-4 pb-3 text-center rounded-top" style={{ borderTop: '4px solid var(--primary-color)' }}>
                    <div className="d-flex flex-column align-items-center justify-content-center">
                        <img src={logoImg} alt="Shell Logo" height="60" className="mb-2" />
                        <img src={textLogoImg} alt="Shell" height="30" />
                    </div>
                </Card.Header>
                <Card.Body className="p-4">

                    <h4 className="text-center text-dark fw-bold mb-4">
                        {isRegistering ? 'Create Account' : 'Welcome Back'}
                    </h4>
                    
                    {error && <div className="alert alert-danger py-2 text-center">{error}</div>}
                    
                    <Form onSubmit={handleSubmit}>
                        {isRegistering && (
                            <>
                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-bold">Full Name</Form.Label>
                                    <Form.Control 
                                        type="text" 
                                        value={clientName} 
                                        onChange={(e) => setClientName(e.target.value)} 
                                        placeholder="John Doe"
                                        required 
                                    />
                                </Form.Group>

                                <Form.Group className="mb-3">
                                    <Form.Label className="fw-bold">Email</Form.Label>
                                    <Form.Control 
                                        type="email" 
                                        value={email} 
                                        onChange={(e) => setEmail(e.target.value)} 
                                        placeholder="john@example.com"
                                        required 
                                    />
                                </Form.Group>
                            </>
                        )}

                        <Form.Group className="mb-3">
                            <Form.Label className="fw-bold">
                                {isRegistering ? 'Phone Number' : 'Email, Username or Phone'}
                            </Form.Label>
                            <Form.Control 
                                type="text" 
                                value={phone} 
                                onChange={(e) => setPhone(e.target.value)} 
                                placeholder={isRegistering ? '+48 123 456 789' : 'user@example.com / +48...'}
                                required 
                            />
                        </Form.Group>
                        
                        <Form.Group className="mb-4">
                            <Form.Label className="fw-bold">Password</Form.Label>
                            <Form.Control 
                                type="password" 
                                value={password} 
                                onChange={(e) => setPassword(e.target.value)} 
                                placeholder="••••••••"
                                required 
                            />
                        </Form.Group>
                        
                        <Button variant="warning" type="submit" className="w-100 fw-bold fs-5 text-dark">
                            {isRegistering ? 'Sign Up' : 'Login'}
                        </Button>
                    </Form>
                    
                    <div className="text-center mt-4">
                        <span className="text-muted">
                            {isRegistering ? 'Already have an account?' : 'Don\'t have an account?'}
                        </span>
                        <Button 
                            variant="link" 
                            className="text-danger fw-bold text-decoration-none ms-1 p-0 pb-1"
                            onClick={() => setIsRegistering(!isRegistering)}
                        >
                            {isRegistering ? 'Login here' : 'Sign up'}
                        </Button>
                    </div>
                </Card.Body>
            </Card>
        </Container>
    );
};

export default Login;
