import SharedDesign from "./SharedDesign/SharedDesign";
import PendingUser from "./Users/PendingUser";
import RegisteredUser from "./Users/RegisteredUser";
import {BrowserRouter} from "react-router-dom";
import React, {useState} from "react";
import Router from "./Misc/Router";
import CookieHandling from "./Misc/CookieHandling";
import registeredUser from "./Users/RegisteredUser";
import Tokens from "./Users/Tokens";

function App() {

    const [userLoggedIn, setUserLoggedIn] = useState(false);
    const [username, setUsername] = useState("");

    return (
        <SharedDesign setUser={setUsername} setLoggedIn={setUserLoggedIn}>
            <BrowserRouter>
                <Router props={{loggedIn: userLoggedIn, setLoggedIn: setUserLoggedIn,
                    username: username, setUsername: setUsername}}/>
            </BrowserRouter>
        </SharedDesign>
    );
}

export default App;
