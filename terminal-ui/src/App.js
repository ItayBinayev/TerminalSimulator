import './App.css';
import React, { useEffect, useState } from 'react';
import signalRService from './services/signalRService';
import Flight from './components/flight';
import * as signalR from '@microsoft/signalr';
import myImage from './resources/airplane-paper.png'; // Path to your image file

//import signalRService from "/services/signalRService.js";


function App() {
  //const [flights, setFlights] = useState([]);
  const [legs, setLegs] = useState([]);
  const image = myImage
  const [signalEvent,setSignalEvent]=React.useState(false)
  useEffect(() => {
    const initializeSignalR = async () => {
     if(!signalEvent)
     {

      signalRService.startConnection();
      
      
      signalRService.addFlightCreatedListener((flight) => {
        console.log("Received flight:", flight);
        updateLeg(flight.currentLeg, flight);
      });
      

      // Handle flightRemove event
      signalRService.addFlightRemoveListener((flightId) => {
        console.log("Removed flight:", flightId);
        removeFlight(flightId);
        // Process the removed flight data
      });
      
      signalRService.addFlightMoveListener((flight, legId) => {
        console.log(`Moved flight: ${flight.flightNumber} - ${legId}`);
        moveFlight(flight, legId);
        // Process the moved flight data
      });
      setSignalEvent(true)
    }
  };
    initializeSignalR();
    }, []);
    const initializeLegs = () => {
      const initialLegs = [];
      for (let i = 1; i <= 9; i++) {
        initialLegs.push({ legNumber: i, flights: [] });
      }
      setLegs(initialLegs);
    };
  
    const updateLeg = (legNumber, flight) => {
      setLegs((prevLegs) =>
      prevLegs.map((leg) =>
        leg.legNumber === legNumber ? { ...leg, flights: [...leg.flights, flight] } : leg
      )
    );
    };
  
    const moveFlight = (flight, updatedLeg) => {
      setLegs((prevLegs) =>
      prevLegs.map((leg) =>
        leg.flights.some((f) => f.id === flight.id)
          ? { ...leg, flights: leg.flights.filter((f) => f.id !== flight.id) }
          : leg
      )
    );
    updateLeg(updatedLeg, flight);
    };
  
    const removeFlight = (flightId) => {
      setLegs((prevLegs) =>
      prevLegs.map((leg) => ({
        ...leg,
        flights: leg.flights.filter((flight) => flight.id !== flightId)
      }))
    );
    };
  
    useEffect(() => {
      initializeLegs();
    }, []);
  

  return (
    <div className="App">
    <div className="terminal">
      <div className="line">
        <div className="leg leg-1">
          <h2>Leg 1</h2>
          {legs[0]?.flights?.length > 0 ? (
            legs[0].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber} 
              <img src={image} width="50px" height="50px" alt="Flight"/>
              </p>
              
              ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-2">
          <h2>Leg 2</h2>
          {legs[1]?.flights?.length > 0 ? (
            legs[1].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
              <img src={image} width="50px" height="50px" alt="Flight"/>

              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-3">
          <h2>Leg 3</h2>
          {legs[2]?.flights?.length > 0 ? (
            legs[2].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
             <img src={image} width="50px" height="50px" alt="Flight"/>

              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-4">
          <h2>Leg 4</h2>
          {legs[3]?.flights?.length > 0 ? (
            legs[3].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
           <img src={image} width="50px" height="50px" alt="Flight"/>
            </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-9">
          <h2>Leg 9</h2>
          {legs[8]?.flights?.length > 0 ? (
            legs[8].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
              <img src={image} width="50px" height="50px" alt="Flight"/>

              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
      </div>
      <div className="line">
      <div className="arrow arrow-right"></div>
        <div className="leg leg-8">
          <h2>Leg 8</h2>
          {legs[7]?.flights?.length > 0 ? (
            legs[7].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
              <img src={image} width="50px" height="50px" alt="Flight"/>
              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-5">
          <h2>Leg 5</h2>
          {legs[4]?.flights?.length > 0 ? (
            legs[4].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
              <img src={image} width="50px" height="50px" alt="Flight"/>

              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
      </div>
      <div className="line">
        <div className="leg leg-7">
          <h2>Leg 7</h2>
          {legs[6]?.flights?.length > 0 ? (
            legs[6].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
               <img src={image} width="50px" height="50px" alt="Flight"/>

              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
        <div className="leg leg-6">
          <h2>Leg 6</h2>
          {legs[5]?.flights?.length > 0 ? (
            legs[5].flights.map((flight) => (
              <p key={flight.id}>Flight: {flight.flightNumber}
              <img src={image} width="50px" height="50px" alt="Flight"/>
              </p>
            ))
          ) : (
            <p>No flights</p>
          )}
        </div>
      </div>
    </div>
    </div>
  );
  
}

export default App;