import React, { useState, useRef } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Home.css';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Home = () => {

    const navigate = useNavigate();
    const [people, setPeople] = useState([]);

    const loadPeople = async () => {
        const { data } = await axios.get('api/people/getall');
        setPeople(data);
    }

    useEffect(() => {
        loadPeople();
    }, [])

    const onDeleteClick = async () => {
        await axios.post('api/people/delete');
        console.log("We here at the Upload People Foundation do not commend nor condemn the actions taken. The usage of this button is dependant on the moral stances of the user, though ethically we cannot ban its universal usage.")
        loadPeople();
        navigate('/');
    }

    return (
        <div className="app-container">
            <div className="d-flex flex-column justify-content-center align-items-center">
            <br/>
                <h1>These are your people</h1>
                <button onClick={onDeleteClick} className="btn btn-danger mb-3 w-100">Remove All Generations! How murderous are you feeling at the moment?</button>
                <table className='table table-striped table-hover col-md-8 w-100'>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Age</th>
                            <th>Email</th>
                        </tr>
                    </thead>
                    <tbody>
                        {people.map(p =>
                            <tr key={p.id}>
                                <td>{p.id}</td>
                                <td>{p.firstName}</td>
                                <td>{p.lastName}</td>
                                <td>{p.age}</td>
                                <td>{p.email}</td>
                            </tr>)}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default Home;