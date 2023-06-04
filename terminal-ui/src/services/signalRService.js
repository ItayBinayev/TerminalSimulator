import * as signalR from "@microsoft/signalr";

const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5192/terminalhub")
  .build();

  const signalRService = {
    startConnection: async () => {
      try {
        if (connection.state === signalR.HubConnectionState.Disconnected) {
          await connection.start();
          console.log("Connected to SignalR hub");
        } else {
          console.log("SignalR connection is already in progress or Server not running.");
        }
      } catch (error) {
        console.error("Error connecting to SignalR hub:", error);
      }
    },

  addFlightCreatedListener: (callback) => {
    connection.on("FlightCreated", callback);
  },

  addFlightRemoveListener: (callback) => {
    connection.on("FlightDismissed", callback);
  },

  addFlightMoveListener: (callback) => {
    connection.on("LegChanged", callback);
  }
};

export default signalRService;