import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import { ContatoVisualizar } from './ContatoVisualizar';
import { ContatoForm } from './ContatoForm';
import 'bootstrap';

interface ContatosDadosState {
    contatos: Contato[];
    idSelecionado: number;
    carregando: boolean;
    apresentando: boolean;
    incluindo: boolean;
    alterando: boolean;
}

export class ContatoDados extends React.Component<RouteComponentProps<{}>, ContatosDadosState> {
    constructor() {
        super();
        this.state = { contatos: [], idSelecionado: 0, carregando: true, apresentando: false, incluindo: false, alterando: false };
        this.atualizarLista = this.atualizarLista.bind(this);
        this.voltarParaLista = this.voltarParaLista.bind(this);
        this.atualizarLista();
    }

    public render() {
        let contents = <p><em>Carregando...</em></p>;
        let modal = <div></div>;

        if (this.state.carregando == false) {
            contents = this.renderContatosTable(this.state.contatos);

            if (this.state.apresentando) {
                modal = <ContatoVisualizar
                    id={this.state.idSelecionado}
                    metodoVoltar={this.voltarParaLista} />
            }
            else if (this.state.incluindo) {
                modal = <ContatoForm
                    id={0}
                    metodoAtualizarLista={this.atualizarLista}
                    metodoVoltar={this.voltarParaLista}
                    incluindo={true} />
            }
            else if (this.state.alterando) { 
                modal = <ContatoForm
                    id={this.state.idSelecionado}
                    metodoAtualizarLista={this.atualizarLista}
                    metodoVoltar={this.voltarParaLista}
                    incluindo={false} />
            }
        }

        return <div>
            <h1>Contatos</h1>
            {contents}
            <div className="modal fade" id="myModal" role="dialog">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <button type="button" className="close" data-dismiss="modal">&times;</button>
                            <h4 className="modal-title">Contato</h4>
                        </div>
                        <div className="modal-body">
                            {modal}
                        </div>
                    </div>
                </div>
            </div>

        </div>;
    }

    private visualizar(id: number) {
        this.setState({
            idSelecionado: id, apresentando: true, incluindo: false, carregando: false, alterando: false
        });
    }

    private incluir() {
        this.setState({
            carregando: false, apresentando: false, alterando: false, incluindo: true
        });
    }

    private alterar(id: number) {
        this.setState({ carregando: false, apresentando: false, incluindo: false, idSelecionado: id, alterando: true
        });
    }

    private atualizarLista() {
        fetch('api/Contato/')
            .then(response => response.json() as Promise<Contato[]>)
            .then(data => {
                this.setState({ contatos: data, carregando: false, apresentando: false, incluindo: false, alterando: false });
            });
    }

    private voltarParaLista() {
        this.atualizarLista();
        //this.setState({ carregando: false, apresentando: false, incluindo: false, alterando: false });
    }

    private gravarExclusao(id: number)
    {
        fetch('api/Contato/' + id, {
            method: 'delete',
            headers: { 'Content-Type': 'application/json' }
        })
        .then(data => {
            this.atualizarLista();
        });
    }

    isValidElement = element => {
        return element.name && element.value;
    };
    isValidValue = element => {
        return (['checkbox', 'radio'].indexOf(element.type) == -1 || element.checked);
    };
    formToJson = elements => [].reduce.call(elements, (data, element) => {
        console.log('formToJson()', element)
        if (this.isValidElement(element) && this.isValidValue(element)) {
            data[element.name] = element.value;
        }
        return data;
    }, {});
    
    private renderContatosTable(contatos: Contato[]) {
        return <div>
            <button className="action btn btn-primary" data-toggle="modal" data-target="#myModal" onClick={(id) => this.incluir()}>Incluir</button>
            <table className='table'>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Nome</th>
                        <th>Email</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {contatos.map(c =>
                        <tr key={c.id}>
                            <td>{c.id}</td>
                            <td>{c.nome}</td>
                            <td>{c.email}</td>
                            <td>
                                <button className="action btn btn-info" data-toggle="modal" data-target="#myModal" onClick={(id) => this.visualizar(c.id)}>Visualizar</button>
                                &nbsp;
                                <button className="action btn btn-warning" data-toggle="modal" data-target="#myModal" onClick={(id) => this.alterar(c.id)}>Alterar</button>
                                &nbsp;
                                <button className="action btn btn-danger" onClick={(id) => this.gravarExclusao(c.id)}>Excluir</button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

}

class Contato {
    id: number;
    nome: string;
    email: string;
    telefone: string;
    telefones: Telefone[];
}
class Telefone {
    id: number;
    idContato: number;
    numero: string;
}
