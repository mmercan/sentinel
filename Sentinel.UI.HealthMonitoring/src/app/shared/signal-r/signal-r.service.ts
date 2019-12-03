import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from '../authentication/auth.service';


@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  public signalRConnected = false;
  public data: [];
  token: '';
  private subject = new Subject<any>();
  constructor(private authService: AuthService) {

  }

  public GetStatus(): Observable<string> {
    return this.subject.asObservable();
  }

  public startConnection = () => {

    // const token = 'Bearer ' + this.authService.getLocalToken();
    const token = this.authService.getLocalToken();

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.signalrBaseUrl}/chat`, { accessTokenFactory: () => token })
      .build();

    Object.defineProperty(WebSocket, 'OPEN', { value: 1 });

    this.hubConnection
      .start()
      .then(() => {
        this.signalRConnected = true;
        console.log('Connection started');
        this.subject.next('connected');
        this.addSendDataListener();

      })
      .catch((err) => {
        console.log('Error while starting connection: ' + JSON.stringify(err));
        this.signalRConnected = false;
        this.subject.next('disconnected');
      });

    this.hubConnection.onclose((err) => {
      this.signalRConnected = false;
      console.log('SignalR connection Closed' + JSON.stringify(err));
      this.subject.next('disconnected');
    });

  }

  public addSendDataListener = () => {
    this.hubConnection.on('Send', (data) => {
      this.data = data;
      console.log(data);
    });
  }
}
