import * as React from 'react';
//import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';

interface ContatoProps {
    id: number;
    metodoAtualizarLista: () => void;
    metodoVoltar: () => void;
    incluindo: boolean;
}

interface ContatoState {
    carregando: boolean;
    contato: Contato;
}

export class ContatoForm extends React.Component<ContatoProps, ContatoState> {
    constructor(props) {
        super(props);

        this.state = { carregando: true, contato: new Contato()};
    }

    public render() {
        let contents = <p>Carregando...</p>;

        if (this.props.incluindo) {
            contents = this.componentRender(this.state.contato);
        }
        else if (this.state.carregando) {
            this.request(this.props.id);
        }
        else { 
            contents = this.componentRender(this.state.contato);
        }
        
        return <div>
                 {contents}
               </div>;
    }

    private request(id: number) {
        fetch('api/Contato/' + id)
            .then(response => response.json() as Promise<Contato>)
            .then(data => {
                this.setState({ contato: data, carregando: false });
            });
    }

    
    private componentRender(obj: Contato) {
        return <div>
                    <form id="formContato">
                        <div className="form-group">
                            <label>{obj.id != null ? 'Id' : ''}</label>
                            <input id='id' readOnly className="form-control" name='id' type={obj.id == null ? 'hidden':'text'} defaultValue={obj.id != null ? (obj.id + '') : ''} />
                        </div>
                        <div className="form-group">
                            <label>Nome</label>
                            <input id='nome' className="form-control" name='nome' type='text' defaultValue={obj.nome != null ? (obj.nome + '') : ''} />
                        </div>
                        <div className="form-group">
                            <label>Email</label>
                            <input id='email' className="form-control" name='email' type='email' defaultValue={obj.email != null ? (obj.email + '') : ''} />
                        </div>
                        <div className="form-group">
                            <label>Novo Telefone</label>
                            <input id='telefone' className="form-control" name='telefone' type='text' defaultValue={obj.telefone != null ? (obj.telefone + '') : ''} />                            
                            <button type="button" className="btn btn-success" onClick={() => this.gravarTelefone(this)}>+</button>
                        </div>                 
                    </form>     
                    <div>
                        <hr/>
                        <table className='table'>
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Telefones</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {obj.telefones.map(t =>
                                    <tr key={t.id}>
                                        <td>{t.id}</td>
                                        <td>{t.numero}</td>
                                        <td>
                                            <button className="action btn btn-danger" onClick={(id) => this.gravarExclusao(t.id)}>Excluir</button>
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>;
                    <div className="modal-footer">
                        <button type="button" className="btn btn-success" data-dismiss="modal" onClick={() => this.gravar(this)}>Gravar</button>
                        <button type="button" className="btn btn-light" data-dismiss="modal"  onClick={() => this.props.metodoVoltar()}>Voltar</button>
                    </div>
                </div>;
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



    private gravar(e) {
        let form: Element = document.querySelector('#formContato')
        
        if (this.props.incluindo) {
            fetch('api/Contato/', {
                method: 'post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.formToJson(form))
            })
            .then(data => {
                this.props.metodoAtualizarLista();
            });
        }
        else {
            let id = document.getElementById('id') as HTMLInputElement
            fetch('api/Contato/' + id, {
                method: 'put',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.formToJson(form))
            })
            .then(data => {
                this.props.metodoAtualizarLista();
            });
        }
    }

    private gravarTelefone(e) {
        let form: Element = document.querySelector('#formContato')

        if (this.props.incluindo) 
        {
            fetch('api/Contato/', {
                method: 'post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.formToJson(form))
            })
                .then(response => response.json() as Promise<Contato>)
                .then(data => {
                    this.setState({ contato: data, carregando: false });
                });
        }
        else {
            let id = document.getElementById('id') as HTMLInputElement
            fetch('api/Contato/' + id, {
                method: 'put',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.formToJson(form))
            })
                .then(data => {
                    this.request(this.props.id);
                });
        }
    }

    private gravarExclusao(idtel: number) {
        fetch('api/Contato/' + this.props.id + '/' + idtel, {
            method: 'delete',
            headers: { 'Content-Type': 'application/json' }
        })
            .then(data => {
                //this.props.metodoAtualizarLista();
            this.request(this.props.id);
        });
    }
   
}

class Contato {
    id: number;
    nome: string;
    email: string;
    telefone: string;
    telefones: Telefone[] = new Array<Telefone>();
}
class Telefone {
    id: number
    idContato: number;
    numero: string;
}
