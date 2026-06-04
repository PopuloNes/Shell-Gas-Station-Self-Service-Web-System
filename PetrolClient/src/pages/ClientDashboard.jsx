import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../services/api';
import { useAuth } from '../contexts/AuthContext';
import logoImg from '../assets/logo.png';
import textLogoImg from '../assets/text_logo.jpg';

const ClientDashboard = () => {
    const { stationId } = useParams();
    const navigate = useNavigate();
    const { user, logout, updateUser } = useAuth();
    
    const [station, setStation] = useState(null);
    const [selectedPumpId, setSelectedPumpId] = useState('');
    const [selectedFuelId, setSelectedFuelId] = useState('');
    const [isVolumeMode, setIsVolumeMode] = useState(false);
    const [inputValue, setInputValue] = useState('');
    const [useBonuses, setUseBonuses] = useState(false);
    const [bonusesToSpend, setBonusesToSpend] = useState('');
    const [paymentCardNumber, setPaymentCardNumber] = useState('');
    
    const [message, setMessage] = useState('');
    const [loading, setLoading] = useState(true);
    const [showCheckout, setShowCheckout] = useState(false);
    const [orderSummary, setOrderSummary] = useState(null);
    const [pumpStarted, setPumpStarted] = useState(false);
    
    // Payment Methods State
    const [savedMethods, setSavedMethods] = useState([]);
    const [selectedMethodId, setSelectedMethodId] = useState('');
    const [isAddingNew, setIsAddingNew] = useState(false);
    const [paymentTab, setPaymentTab] = useState('card'); // 'card', 'blik', 'crypto'
    const [saveNewMethod, setSaveNewMethod] = useState(true);

    const [cardExpiry, setCardExpiry] = useState('');
    const [cardCVC, setCardCVC] = useState('');
    const [blikCode, setBlikCode] = useState('');
    const [cryptoAddress, setCryptoAddress] = useState('');

    useEffect(() => {
        const fetchStation = async () => {
            try {
                const res = await api.get(`/GasStation/${stationId}/details`);
                setStation(res.data);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };
        fetchStation();
    }, [stationId]);

    const loadPaymentMethods = async () => {
        if (!user) return;
        try {
            const res = await api.get(`/PaymentMethod/user/${user.id}`);
            setSavedMethods(res.data);
            if (res.data.length > 0) {
                setIsAddingNew(false);
                const def = res.data.find(m => m.isDefault) || res.data[0];
                setSelectedMethodId(def.id);
            } else {
                setIsAddingNew(true);
            }
        } catch (err) {
            console.error("Failed to load payment methods", err);
        }
    };

    useEffect(() => {
        if (showCheckout) {
            loadPaymentMethods();
        }
    }, [showCheckout]);

    const handleCheckoutClick = (e) => {
        e.preventDefault();
        setMessage('');
        
        if (!selectedPumpId || !selectedFuelId || !inputValue || parseFloat(inputValue) <= 0) {
            setMessage('Please select a pump, fuel, and enter a valid quantity.');
            return;
        }

        if (useBonuses) {
            const bonuses = parseInt(bonusesToSpend);
            if (isNaN(bonuses) || bonuses <= 0) {
                setMessage('Please enter a valid amount of bonuses to spend.');
                return;
            }
            if (bonuses > (user?.bonusBalance || 0)) {
                setMessage(`You only have ${user?.bonusBalance || 0} bonuses.`);
                return;
            }
        }
        
        setShowCheckout(true);
    };

    const handleCardNumberChange = (e) => {
        let raw = e.target.value.replace(/\D/g, '');
        let formatted = raw.match(/.{1,4}/g)?.join(' ') || raw;
        setPaymentCardNumber(formatted);
    };

    const handleExpiryChange = (e) => {
        let raw = e.target.value.replace(/\D/g, '');
        
        // Month validation
        if (raw.length >= 2) {
            let month = parseInt(raw.substring(0, 2), 10);
            if (month < 1) month = 1;
            if (month > 12) month = 12;
            let monthStr = month.toString().padStart(2, '0');
            raw = monthStr + raw.substring(2);
        }

        if (raw.length > 2) {
            raw = raw.slice(0, 2) + '/' + raw.slice(2, 4);
        }
        setCardExpiry(raw);
    };

    const handleDeleteMethod = async (id) => {
        try {
            await api.delete(`/PaymentMethod/${id}`);
            loadPaymentMethods();
        } catch(err) {
            alert('Failed to delete method');
        }
    };

    const handleProcessOrder = async () => {
        try {
            let finalCardStr = "";
            if (isAddingNew) {
                let details = "";
                let type = "";
                if (paymentTab === 'card') {
                    const rawCard = paymentCardNumber.replace(/\D/g, '');
                    if (rawCard.length !== 16) return alert('Card Number must be exactly 16 digits');
                    if (cardExpiry.length < 5) return alert('Invalid Expiry');
                    if (cardCVC.length < 3) return alert('Invalid CVC');
                    type = "Visa/Mastercard";
                    details = `**** **** **** ${rawCard.slice(-4)} (Exp: ${cardExpiry})`;
                } else if (paymentTab === 'blik') {
                    if (blikCode.length !== 6) return alert('Blik code must be exactly 6 digits');
                    type = "Blik";
                    details = `Code: ${blikCode}`;
                } else if (paymentTab === 'crypto') {
                    if (!cryptoAddress || cryptoAddress.length < 26) return alert('Crypto wallet address must be at least 26 characters long');
                    type = "Crypto";
                    details = `Wallet: ${cryptoAddress}`;
                }
                
                if (saveNewMethod && user) {
                    const res = await api.post('/PaymentMethod', {
                        userId: user.id,
                        type: type,
                        details: details,
                        isDefault: savedMethods.length === 0
                    });
                    finalCardStr = `${res.data.type}: ${res.data.details}`;
                } else {
                    finalCardStr = `${type}: ${details}`;
                }
            } else {
                const sel = savedMethods.find(m => m.id === selectedMethodId);
                if (!sel) return alert('Select a method');
                finalCardStr = `${sel.type}: ${sel.details}`;
            }

            const req = {
                userId: user.id,
                gasStationId: parseInt(stationId),
                pumpId: parseInt(selectedPumpId),
                fuelTypeId: parseInt(selectedFuelId),
                inputValue: parseFloat(inputValue),
                isVolumeMode: isVolumeMode,
                useBonuses: useBonuses,
                bonusesToSpend: useBonuses ? parseInt(bonusesToSpend) || 0 : 0,
                paymentCardNumber: finalCardStr
            };
            
            const res = await api.post('/Order/process', req);
            setOrderSummary(res.data);
            setShowCheckout(false);

            if (updateUser) {
                const spent = useBonuses ? (parseInt(bonusesToSpend) || 0) : 0;
                const newBalance = (user?.bonusBalance || 0) - spent + (res.data.bonusesEarned || 0);
                updateUser({ ...user, bonusBalance: newBalance });
            }
        } catch (err) {
            setMessage(`Error: ${err.response?.data?.message || err.response?.data || err.message}`);
            setShowCheckout(false);
        }
    };
    
    const startPump = () => {
        setPumpStarted(true);
    };

    const finishAndClose = () => {
        setOrderSummary(null);
        setPumpStarted(false);
        setInputValue('');
        setUseBonuses(false);
        setBonusesToSpend('');
        setSelectedFuelId('');
        setSelectedPumpId('');
        setMessage('Thanks for using our service!');
        
        const fetchStation = async () => {
            try {
                const res = await api.get(`/GasStation/${stationId}/details`);
                setStation(res.data);
            } catch (err) {}
        };
        fetchStation();
    };

    if (loading) return <div style={{ textAlign: 'center', marginTop: '50px' }}>Loading...</div>;
    if (!station) return <div style={{ textAlign: 'center', marginTop: '50px' }}>Station not found.</div>;

    return (
        <div className="container py-4 min-vh-100 position-relative" style={{ maxWidth: '800px' }}>
            <header className="bg-white shadow-sm p-3 mb-4 rounded d-flex justify-content-between align-items-center" style={{ borderBottom: '4px solid var(--primary-color)' }}>
                <div className="d-flex align-items-center">
                    <img src={logoImg} alt="Shell Logo" height="50" className="me-2" />
                    <img src={textLogoImg} alt="Shell" height="30" />
                </div>
                
                <div className="d-flex align-items-center gap-3">
                    <button className="btn btn-outline-danger fw-bold btn-sm" onClick={() => navigate('/map')}>
                        &larr; Map
                    </button>
                    <div className="text-end">
                        <div className="fw-bold text-dark">{user?.username}</div>
                        <div className="text-warning fw-bold bg-dark px-2 rounded-pill small">
                            {user?.bonusBalance || 0} pts
                        </div>
                    </div>
                    <button className="btn btn-danger fw-bold btn-sm" onClick={logout}>Logout</button>
                </div>
            </header>

            <div className="card shadow-sm mb-4 border-0">
                <div className="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h1 className="fw-bold text-dark m-0">{station.name}</h1>
                        <p className="text-muted m-0 mt-1"><i className="bi bi-geo-alt-fill text-danger me-2"></i>{station.address}</p>
                    </div>
                </div>
            </div>

            <div className="card shadow-sm mb-4 border-0">
                <div className="card-body">
                    <h2 className="card-title text-danger mb-4 fw-bold">Create Order</h2>
                    
                    {message && (
                        <div className="alert alert-info border-primary">
                            {message}
                        </div>
                    )}
                    
                    <form onSubmit={handleCheckoutClick}>
                        
                        <div className="mb-3">
                            <label className="form-label fw-bold text-dark">Select Pump</label>
                            <select 
                                className="form-select border-danger"
                                value={selectedPumpId} 
                                onChange={e => {
                                    setSelectedPumpId(e.target.value);
                                    setSelectedFuelId(''); // Clear fuel selection when pump changes
                                }} 
                                required
                            >
                                <option value="">-- Choose Pump --</option>
                                {station.pumps?.map(p => (
                                    <option key={p.id} value={p.id} disabled={p.status !== 2}>
                                        {p.name} {p.status !== 2 ? '(Closed)' : ''}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="mb-3">
                            <label className="form-label fw-bold text-dark">Select Fuel</label>
                            <select 
                                className="form-select border-warning"
                                value={selectedFuelId} 
                                onChange={e => setSelectedFuelId(e.target.value)} 
                                required
                                disabled={!selectedPumpId}
                            >
                                <option value="">-- Choose Fuel --</option>
                                {station.availableFuels?.filter(f => !selectedPumpId || f.pumpIds?.includes(parseInt(selectedPumpId))).map(f => {
                                    const isAvailable = f.pumpIds && station.pumps?.some(p => p.status === 2 && f.pumpIds.includes(p.id));
                                    return (
                                        <option key={f.fuelTypeId} value={f.fuelTypeId} disabled={!isAvailable}>
                                            {f.fuelName} - {isAvailable ? `${f.price} PLN/L (Available: ${f.availableVolume} L)` : 'Not Available'}
                                        </option>
                                    );
                                })}
                            </select>
                        </div>

                        <div className="mb-3 d-flex gap-4">
                            <div className="form-check">
                                <input 
                                    className="form-check-input border-danger"
                                    type="radio" 
                                    name="mode" 
                                    id="modeAmount"
                                    checked={!isVolumeMode} 
                                    onChange={() => setIsVolumeMode(false)}
                                />
                                <label className="form-check-label fw-bold" htmlFor="modeAmount">By Amount (PLN)</label>
                            </div>
                            <div className="form-check">
                                <input 
                                    className="form-check-input border-warning"
                                    type="radio" 
                                    name="mode" 
                                    id="modeVolume"
                                    checked={isVolumeMode} 
                                    onChange={() => setIsVolumeMode(true)}
                                />
                                <label className="form-check-label fw-bold" htmlFor="modeVolume">By Volume (Liters)</label>
                            </div>
                        </div>

                        <div className="mb-3">
                            <label className="form-label fw-bold text-dark">{isVolumeMode ? 'Volume (L)' : 'Total Amount (PLN)'}</label>
                            <input 
                                className="form-control"
                                type="number" 
                                step="0.01" 
                                value={inputValue} 
                                onChange={e => setInputValue(e.target.value)} 
                                required 
                                placeholder={isVolumeMode ? 'e.g. 20' : 'e.g. 150.00'}
                            />
                        </div>

                        <div className="mb-3 form-check">
                            <input 
                                className="form-check-input"
                                type="checkbox" 
                                id="payWithBonuses"
                                checked={useBonuses} 
                                onChange={e => setUseBonuses(e.target.checked)} 
                            />
                            <label className="form-check-label text-dark fw-bold" htmlFor="payWithBonuses">Pay with bonuses</label>
                        </div>

                        {useBonuses && (
                            <div className="mb-3">
                                <label className="form-label fw-bold text-dark">Bonuses to spend</label>
                                <input 
                                    className="form-control border-success"
                                    type="number" 
                                    step="1" 
                                    value={bonusesToSpend} 
                                    onChange={e => setBonusesToSpend(e.target.value)} 
                                    max={user?.bonusBalance}
                                    placeholder={`Max: ${user?.bonusBalance || 0}`}
                                    required={useBonuses}
                                />
                            </div>
                        )}

                        <button type="submit" className="btn btn-danger btn-lg mt-3 fw-bold w-100 shadow-sm">
                            Checkout
                        </button>
                    </form>
                </div>
            </div>

            {/* STRIPE-LIKE PAYMENT MODAL */}
            {showCheckout && (
                <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, background: 'rgba(0,0,0,0.7)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000 }}>
                    <div className="card shadow-lg border-0" style={{ width: '95%', maxWidth: '1200px', maxHeight: '90vh', overflowY: 'auto', padding: '40px' }}>
                        <h2 className="text-dark fw-bold text-center mb-4" style={{ fontSize: '2.5rem' }}>Secure Payment</h2>
                        
                        {/* Toggles */}
                        {savedMethods.length > 0 && (
                            <div className="d-flex mb-4 gap-3">
                                <button className={`btn flex-fill fs-5 fw-bold py-3 ${!isAddingNew ? 'btn-danger text-white shadow-sm' : 'btn-outline-danger'}`} onClick={() => setIsAddingNew(false)}>Saved Methods</button>
                                <button className={`btn flex-fill fs-5 fw-bold py-3 ${isAddingNew ? 'btn-danger text-white shadow-sm' : 'btn-outline-danger'}`} onClick={() => setIsAddingNew(true)}>Add New</button>
                            </div>
                        )}

                        {!isAddingNew && savedMethods.length > 0 && (
                            <div className="mb-4">
                                <label className="form-label text-muted fw-bold fs-5 mb-3">Select Payment Method</label>
                                {savedMethods.map(m => (
                                    <div key={m.id} 
                                        className={`d-flex align-items-center justify-content-between p-4 border rounded mb-3 ${selectedMethodId === m.id ? 'border-danger bg-danger bg-opacity-10 shadow-sm' : 'bg-light'}`}
                                        style={{ cursor: 'pointer', transition: 'all 0.2s' }}
                                        onClick={() => setSelectedMethodId(m.id)}>
                                        <div>
                                            <div className="fw-bold text-dark fs-4 mb-1">{m.type}</div>
                                            <div className="text-muted fs-5">{m.details}</div>
                                        </div>
                                        <button className="btn btn-outline-danger btn-lg fw-bold" onClick={(e) => { e.stopPropagation(); handleDeleteMethod(m.id); }}>Delete</button>
                                    </div>
                                ))}
                            </div>
                        )}

                        {isAddingNew && (
                            <div className="mb-4">
                                <div className="d-flex border-bottom mb-4">
                                    <button onClick={() => setPaymentTab('card')} className={`btn flex-fill fs-4 fw-bold p-3 rounded-0 ${paymentTab === 'card' ? 'text-danger border-danger border-bottom border-3' : 'text-muted border-0'}`}>Card</button>
                                    <button onClick={() => setPaymentTab('blik')} className={`btn flex-fill fs-4 fw-bold p-3 rounded-0 ${paymentTab === 'blik' ? 'text-danger border-danger border-bottom border-3' : 'text-muted border-0'}`}>Blik</button>
                                    <button onClick={() => setPaymentTab('crypto')} className={`btn flex-fill fs-4 fw-bold p-3 rounded-0 ${paymentTab === 'crypto' ? 'text-danger border-danger border-bottom border-3' : 'text-muted border-0'}`}>Crypto</button>
                                </div>

                                {paymentTab === 'card' && (
                                    <div>
                                        <label className="form-label text-muted fw-bold fs-5 mb-2">Card Information</label>
                                        <div className="border rounded bg-light p-2 shadow-sm">
                                            <input type="text" placeholder="Card Number" value={paymentCardNumber} onChange={handleCardNumberChange} maxLength={19} className="form-control fs-4 border-0 border-bottom border-secondary border-opacity-25 bg-transparent mb-2 p-3 shadow-none text-dark" />
                                            <div className="d-flex">
                                                <input type="text" placeholder="MM/YY" value={cardExpiry} onChange={handleExpiryChange} maxLength={5} className="form-control fs-4 border-0 border-end border-secondary border-opacity-25 bg-transparent p-3 shadow-none text-dark" />
                                                <input type="text" placeholder="CVC" value={cardCVC} onChange={e => setCardCVC(e.target.value.replace(/\D/g, ''))} maxLength={3} className="form-control fs-4 border-0 bg-transparent p-3 shadow-none text-dark" />
                                            </div>
                                        </div>
                                    </div>
                                )}

                                {paymentTab === 'blik' && (
                                    <div>
                                        <label className="form-label text-muted fw-bold fs-5 mb-2">6-Digit Blik Code</label>
                                        <input type="text" placeholder="000 000" value={blikCode} onChange={e => setBlikCode(e.target.value.replace(/\D/g, ''))} maxLength={6} className="form-control fs-1 text-center py-4 bg-light fw-bold text-dark shadow-sm border" style={{ letterSpacing: '16px' }} />
                                    </div>
                                )}

                                {paymentTab === 'crypto' && (
                                    <div>
                                        <label className="form-label text-muted fw-bold fs-5 mb-2">Wallet Address</label>
                                        <input type="text" placeholder="0x..." value={cryptoAddress} onChange={e => setCryptoAddress(e.target.value)} className="form-control fs-4 py-4 bg-light text-dark shadow-sm border" />
                                    </div>
                                )}

                                {user && paymentTab !== 'blik' && (
                                    <label className="form-check d-flex align-items-center mt-4 fs-5 text-muted cursor-pointer" style={{ cursor: 'pointer' }}>
                                        <input className="form-check-input me-3 shadow-sm border-secondary" type="checkbox" checked={saveNewMethod} onChange={e => setSaveNewMethod(e.target.checked)} style={{ width: '28px', height: '28px' }} />
                                        <span className="fw-bold ms-2">Save this method for future purchases</span>
                                    </label>
                                )}
                            </div>
                        )}

                        <div className="d-flex gap-4 mt-5">
                            <button className="btn btn-outline-secondary fs-4 fw-bold py-3 flex-fill" onClick={() => setShowCheckout(false)}>Cancel</button>
                            <button className="btn btn-warning fs-4 fw-bold py-3 text-dark flex-fill shadow-sm" onClick={handleProcessOrder}>Pay Now</button>
                        </div>
                    </div>
                </div>
            )}

            {/* Success Summary Popup */}
            {orderSummary && (
                <div style={{ position: 'fixed', top: 0, left: 0, right: 0, bottom: 0, background: 'rgba(0,0,0,0.8)', display: 'flex', justifyContent: 'center', alignItems: 'center', zIndex: 1000 }}>
                    <div className="card shadow-lg border-0" style={{ width: '95%', maxWidth: '1000px', padding: '48px', maxHeight: '90vh', overflowY: 'auto' }}>
                        {!pumpStarted ? (
                            <>
                                <h2 className="text-success text-center mb-5 fw-bold" style={{ fontSize: '3rem' }}>Płatność zakończona sukcesem!</h2>
                                <div className="bg-light p-5 rounded mb-5 shadow-sm border">
                                    <h3 className="mb-4 fs-3 fw-bold border-bottom border-2 pb-3 text-dark">Podsumowanie zamówienia</h3>
                                    <div className="d-flex justify-content-between mb-4 fs-4"><span className="text-muted fw-bold">Paliwo:</span> <strong className="text-dark fs-3">{orderSummary.fuelName}</strong></div>
                                    <div className="d-flex justify-content-between mb-4 fs-4"><span className="text-muted fw-bold">Ilość:</span> <strong className="text-dark fs-3">{orderSummary.volume} L</strong></div>
                                    <div className="d-flex justify-content-between mb-4 fs-4"><span className="text-muted fw-bold">Suma częściowa:</span> <strong className="text-dark fs-3">{orderSummary.amountBeforeDiscount} PLN</strong></div>
                                    <div className="d-flex justify-content-between mb-4 fs-4"><span className="text-muted fw-bold">Wykorzystane punkty:</span> <strong className="text-danger fs-3">-{useBonuses ? bonusesToSpend : 0}</strong></div>
                                    <div className="d-flex justify-content-between mb-4 fs-4"><span className="text-muted fw-bold">Metoda płatności:</span> <strong className="text-dark fs-3">{orderSummary.paymentMethod || 'Karta'}</strong></div>
                                    <div className="d-flex justify-content-between mb-4 pt-4 border-top border-2 fs-3"><span className="text-muted fw-bold">Razem zapłacono:</span> <strong className="text-dark fw-bold fs-1">{orderSummary.totalPaid} PLN</strong></div>
                                    <div className="d-flex justify-content-between mt-5 fs-3 text-success fw-bold"><span>Zdobyte punkty:</span> <span>+{orderSummary.bonusesEarned}</span></div>
                                </div>
                                <button className="btn btn-warning w-100 py-4 fs-3 fw-bold text-dark rounded shadow" onClick={startPump}>Uruchom dystrybutor</button>
                            </>
                        ) : (
                            <div className="text-center p-5">
                                <h2 className="text-primary mb-4 fw-bold" style={{ fontSize: '3rem' }}>Dystrybutor pracuje...</h2>
                                <div style={{ fontSize: '100px', marginBottom: '32px', animation: 'pulse 2s infinite' }}>⛽</div>
                                <p className="mb-5 text-muted fw-bold" style={{ fontSize: '2rem' }}>Wydawanie <span className="text-dark">{orderSummary.volume} L</span> paliwa {orderSummary.fuelName}</p>
                                <button className="btn btn-primary w-100 py-4 fs-3 fw-bold rounded shadow" onClick={finishAndClose}>Zakończ</button>
                            </div>
                        )}
                    </div>
                </div>
            )}
            
            <style>{`
                @keyframes pulse {
                    0% { opacity: 1; transform: scale(1); }
                    50% { opacity: 0.7; transform: scale(1.1); }
                    100% { opacity: 1; transform: scale(1); }
                }
            `}</style>
        </div>
    );
};

export default ClientDashboard;
