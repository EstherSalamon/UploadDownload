import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const Generate = () => {

    const navigate = useNavigate();
    const [amount, setAmount] = useState('');

    const onAmountChange = e => {
        setAmount(e.target.value);
    }

    const onGenerateClick = () => {
        /*await axios.get('api/people/download', { Amount: amount });*/
        /*navigate('/');*/
        window.location.href = `/api/people/download?amount=${amount}`;
        navigate('/');
    }

    return (
        <div className="d-flex vh-100" style={{ marginTop: -70 }}>
            <div className="d-flex w-100 justify-content-center align-self-center">
                <div className="row">
                    <div className="col-md-10">
                         <input type='text' name='amount' value={amount} onChange={onAmountChange} placeholder='Amount' className='form-control'></input>
                    </div>
                    <div className='col-md-2'>
                       <button className='btn btn-outline-info' onClick={onGenerateClick}>Generate</button>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Generate;