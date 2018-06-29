import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { ContatoDados } from './components/ContatoDados';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
    <Route path='/contatodados' component={ContatoDados} />
</Layout>;
