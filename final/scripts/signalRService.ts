import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

export default class SignalRService {
  private url: string;
  private connection = ref<HubConnection | null>(null);
  public bars = ref<string>('');

  constructor() {
    const baseUrl =
      window.location.hostname === 'localhost' ||
      location.hostname === '172.31.112.1'
        ? 'https://localhost:7166'
        : 'AZURE-API-WHEN-CREATED';

    this.url = baseUrl + '/hub';
    this.startConnection();
    this.onReceiveBar('ReceiveBar', (bar: string) => {
      console.log(bar);
      console.log(this.bars.value);
      this.bars.value += bar + '\n';
    });
  }

  private startConnection = async () => {
    this.connection.value = new HubConnectionBuilder()
      .withUrl(this.url)
      .build();

    this.connection.value.onclose((error: any) => {
      console.error('Connection Closed', error);
    });

    try {
      await this.connection.value.start();
      console.log('Connected!');
    } catch (error) {
      console.error('Connection Error: ', error);
    }
  };

  private onReceiveBar = (
    methodName: string,
    callback: (...args: any[]) => void
  ) => {
    this.connection.value?.on(methodName, callback);
  };

  public sendBar(bar: string) {
    if (this.connection.value) {
      this.connection.value.invoke('SendBar', bar);
    }
  }

  public stopConnection = async () => {
    if (!this.connection.value) return;
    await this.connection.value.stop();
    console.log('Disconnected!');
  };
}
