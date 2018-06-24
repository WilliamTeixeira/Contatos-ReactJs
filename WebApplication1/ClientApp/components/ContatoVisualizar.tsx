import * as React from 'react';
//import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface ContatoProps {
    id: number;
    metodoVoltar: () => void;
}

interface ContatoState {
    carregando: boolean;
    contato: Contato;
}

export class ContatoVisualizar extends React.Component<ContatoProps, ContatoState> {
    constructor(props) {
        super(props);

        this.state = { carregando: true, contato: new Contato() };
    }

    public render() {
        let contents = <p>Carregando...</p>;

        if (this.state.carregando)
            this.visualizar(this.props.id);
        else //(this.state.carregando == false)
            contents = this.renderContato(this.state.contato); 

        return <div>
            {contents}
        </div>;
    }

    private voltarParaInicial() {
    }

    private visualizar(id: number) {
        fetch('api/Contato/' + id)
            .then(response => response.json() as Promise<Contato>)
            .then(data => {
                this.setState({ contato: data, carregando: false });
            });
    }

    private renderContato(obj: Contato) {
        return <div>
            <form id="formContato">
                <div className="form-group">
                    <label>Id</label>
                    <input id='id' readOnly={true} className="form-control" name='id' type='text' defaultValue={obj.id != null ? (obj.id + '') : ''} />
                </div>
                <div className="form-group">
                    <label>Nome</label>
                    <input id='nome' readOnly className="form-control" name='nome' type='text' defaultValue={obj.nome != null ? (obj.nome + '') : ''} />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input id='email' readOnly className="form-control" name='email' type='text' defaultValue={obj.email != null ? (obj.email + '') : ''} />
                </div>
                <div className="form-group">
                    <label>Telefone</label>
                    <input id='telefone' readOnly className="form-control" name='telefone' type='text' defaultValue={obj.telefone != null ? (obj.telefone + '') : ''} />
                </div>
            </form>
            <div className="modal-footer">
                <button type="button" className="btn btn-light" data-dismiss="modal"  onClick={() => this.props.metodoVoltar()}>Voltar</button>
            </div>
        </div>;
    }
}

class Contato {
    id: number;
    nome: string;
    email: string;
    telefone: string;
}