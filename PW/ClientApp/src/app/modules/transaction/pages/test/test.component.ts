import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../../../../../environments/environment';

@Component({
    selector: 'app-test',
    templateUrl: './test.component.html'
})
export class TestComponent implements OnInit {
    public hubConnection: HubConnection;
    public messages: string[] = [];
    public message: string;

    ngOnInit() {
        let builder = new HubConnectionBuilder();

        // as per setup in the startup.cs
        this.hubConnection = builder.withUrl(environment.apiUrl + '/hubs/loopy').build();

        // message coming from the server
        this.hubConnection.on('Send', (message) => {
            this.messages.push(message);
        });

        // starting the connection
        this.hubConnection.start();
    }

    send() {
        // message sent from the client to the server
        this.hubConnection.invoke('Echo', this.message);
        this.message = '';
    }
}
