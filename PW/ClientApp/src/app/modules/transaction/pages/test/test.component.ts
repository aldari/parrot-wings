import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth.service';

@Component({
    selector: 'app-test',
    templateUrl: './test.component.html'
})
export class TestComponent implements OnInit, OnDestroy {
    public hubConnection: HubConnection;
    public messages: string[] = [];
    public message: string;

    constructor(private authService: AuthService) {}

    ngOnInit() {
        let builder = new HubConnectionBuilder();

        // as per setup in the startup.cs
        const auth = this.authService.getAuth();
        const auth_token = auth === null ? null : auth['auth_token'];
        console.log(auth_token);
        this.hubConnection = builder
            .withUrl(environment.apiUrl + '/hubs/loopy', { accessTokenFactory: () => auth_token })
            .build();

        // message coming from the server
        this.hubConnection.on('TransactionNotify', (message) => {
            console.log(message);
        });

        // starting the connection
        this.hubConnection.start();
    }

    ngOnDestroy(): void {
        //this.hubConnection.invoke('Leave');
    }
}
