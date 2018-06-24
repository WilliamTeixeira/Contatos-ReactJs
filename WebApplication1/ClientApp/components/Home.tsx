import * as React from 'react';
import { RouteComponentProps } from 'react-router';

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Persiste Contatos</h1>
            <p>Programa para persistencia de contatos!</p>
            <p>Um contato possui:</p>
            <ul>
                <li>Nome</li>
                <li>Email</li>
                <li>Telefone</li>
            </ul>
            
            <h4>Desenvolvido por:</h4>
            <small>
                William e Hugo
            </small>
        </div>;
    }
}
