import React from 'react';

const Flight = ({ flight }) => {
  return (
    <div>
      <h3>Flight Details</h3>
      <p>Flight ID: {flight.id}</p>
      <p>Flight Name: {flight.name}</p>
      {/* Add more flight information as needed */}
    </div>
  );
};

export default Flight;
