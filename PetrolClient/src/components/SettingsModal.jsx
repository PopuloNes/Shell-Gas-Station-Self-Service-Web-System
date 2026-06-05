import React, { useState, useEffect } from 'react';
import { Modal, Button, ListGroup, Card, Form } from 'react-bootstrap';
import api from '../services/api';
import { useAuth } from '../contexts/AuthContext';

const SettingsModal = ({ show, onHide }) => {
    const { user, logout } = useAuth();
    const [view, setView] = useState('menu'); // 'menu', 'receipts', 'payments'
    
    // Data states
    const [receipts, setReceipts] = useState([]);
    const [payments, setPayments] = useState([]);
    
    // New Payment state
    const [newPaymentType, setNewPaymentType] = useState('card');
    const [newPaymentDetails, setNewPaymentDetails] = useState('');

    useEffect(() => {
        if (show) {
            setView('menu');
        }
    }, [show]);

    const fetchReceipts = async () => {
        try {
            const res = await api.get(`/Order/user/${user.id}`);
            setReceipts(res.data);
            setView('receipts');
        } catch (error) {
            console.error("Error fetching receipts", error);
            alert("Failed to fetch receipts");
        }
    };

    const fetchPayments = async () => {
        try {
            const res = await api.get(`/PaymentMethod/user/${user.id}`);
            setPayments(res.data);
            setView('payments');
        } catch (error) {
            console.error("Error fetching payments", error);
            alert("Failed to fetch payment methods");
        }
    };

    const handleAddPayment = async (e) => {
        e.preventDefault();
        try {
            await api.post('/PaymentMethod', {
                userId: user.id,
                type: newPaymentType,
                details: newPaymentDetails,
                isDefault: payments.length === 0
            });
            setNewPaymentDetails('');
            fetchPayments();
        } catch (error) {
            console.error("Error adding payment", error);
            alert("Failed to add payment method");
        }
    };

    const handleDeletePayment = async (id) => {
        try {
            await api.delete(`/PaymentMethod/${id}`);
            fetchPayments();
        } catch (error) {
            console.error("Error deleting payment", error);
            alert("Failed to delete payment method");
        }
    };

    return (
        <Modal show={show} onHide={onHide} centered>
            <Modal.Header closeButton className="bg-light">
                <Modal.Title className="text-danger fw-bold">
                    {view === 'menu' && 'Settings'}
                    {view === 'receipts' && 'My Receipts'}
                    {view === 'payments' && 'Payment Methods'}
                </Modal.Title>
            </Modal.Header>
            <Modal.Body className="bg-light">
                {view === 'menu' && (
                    <div className="d-flex flex-column gap-3">
                        <Button variant="outline-danger" onClick={fetchReceipts} size="lg">
                            View your previous receipts
                        </Button>
                        <Button variant="outline-danger" onClick={fetchPayments} size="lg">
                            Manage payment methods
                        </Button>
                        <Button variant="danger" onClick={logout} size="lg">
                            Logout
                        </Button>
                    </div>
                )}

                {view === 'receipts' && (
                    <div>
                        <Button variant="link" className="text-danger mb-3 p-0" onClick={() => setView('menu')}>
                            &larr; Back to Settings
                        </Button>
                        {receipts.length === 0 ? (
                            <p className="text-muted">No previous receipts found.</p>
                        ) : (
                            <div style={{ maxHeight: '400px', overflowY: 'auto' }}>
                                {receipts.map(r => (
                                    <Card key={r.orderId} className="mb-3 border-danger">
                                        <Card.Body>
                                            <Card.Title className="text-danger">{r.fuelName} - {r.volume} L</Card.Title>
                                            <Card.Subtitle className="mb-2 text-muted">{new Date(r.date).toLocaleString()}</Card.Subtitle>
                                            <ListGroup variant="flush">
                                                <ListGroup.Item className="d-flex justify-content-between">
                                                    <span>Kwota:</span> <strong>{r.amountBeforeDiscount} PLN</strong>
                                                </ListGroup.Item>
                                                <ListGroup.Item className="d-flex justify-content-between">
                                                    <span>Wykorzystane punkty:</span> <strong className="text-danger">-{r.bonusSpent}</strong>
                                                </ListGroup.Item>
                                                <ListGroup.Item className="d-flex justify-content-between">
                                                    <span>Metoda płatności:</span> <strong>{r.paymentMethod}</strong>
                                                </ListGroup.Item>
                                                <ListGroup.Item className="d-flex justify-content-between border-top border-danger pt-2">
                                                    <span>Razem zapłacono:</span> <strong className="fs-5">{r.totalPaid} PLN</strong>
                                                </ListGroup.Item>
                                                <ListGroup.Item className="d-flex justify-content-between text-success">
                                                    <span>Zdobyte punkty:</span> <strong>+{r.bonusesEarned}</strong>
                                                </ListGroup.Item>
                                            </ListGroup>
                                        </Card.Body>
                                    </Card>
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {view === 'payments' && (
                    <div>
                        <Button variant="link" className="text-danger mb-3 p-0" onClick={() => setView('menu')}>
                            &larr; Back to Settings
                        </Button>
                        
                        <h5 className="text-danger">Current Methods</h5>
                        {payments.length === 0 ? (
                            <p className="text-muted">No payment methods saved.</p>
                        ) : (
                            <ListGroup className="mb-4">
                                {payments.map(p => (
                                    <ListGroup.Item key={p.id} className="d-flex justify-content-between align-items-center">
                                        <div>
                                            <strong>{p.type.toUpperCase()}</strong>
                                            <span className="ms-2 text-muted">{p.details}</span>
                                            {p.isDefault && <span className="badge bg-success ms-2">Default</span>}
                                        </div>
                                        <Button variant="outline-danger" size="sm" onClick={() => handleDeletePayment(p.id)}>
                                            Remove
                                        </Button>
                                    </ListGroup.Item>
                                ))}
                            </ListGroup>
                        )}

                        <Card className="border-warning">
                            <Card.Body>
                                <Card.Title className="text-warning">Add New Method</Card.Title>
                                <Form onSubmit={handleAddPayment}>
                                    <Form.Group className="mb-3">
                                        <Form.Label>Type</Form.Label>
                                        <Form.Select value={newPaymentType} onChange={e => setNewPaymentType(e.target.value)}>
                                            <option value="card">Credit/Debit Card</option>
                                            <option value="crypto">Crypto Wallet</option>
                                        </Form.Select>
                                    </Form.Group>
                                    <Form.Group className="mb-3">
                                        <Form.Label>Details (e.g. Card Number)</Form.Label>
                                        <Form.Control 
                                            type="text" 
                                            required 
                                            value={newPaymentDetails} 
                                            onChange={e => setNewPaymentDetails(e.target.value)}
                                            placeholder="Enter details..."
                                        />
                                    </Form.Group>
                                    <Button variant="warning" type="submit" className="w-100 fw-bold">
                                        Add Method
                                    </Button>
                                </Form>
                            </Card.Body>
                        </Card>
                    </div>
                )}
            </Modal.Body>
        </Modal>
    );
};

export default SettingsModal;
