import { ActivatedRoute } from '@angular/router';
import { GestionarParametricasService } from './../../../../core/_services/gestionarParametricas/gestionar-parametricas.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-consultar-editar-parametricas',
  templateUrl: './consultar-editar-parametricas.component.html',
  styleUrls: ['./consultar-editar-parametricas.component.scss']
})
export class ConsultarEditarParametricasComponent implements OnInit {

    constructor(
        private gestionarParametricasSvc: GestionarParametricasService,
        private activatedRoute: ActivatedRoute )
    {
      this.gestionarParametricasSvc.dominioByIdDominio( this.activatedRoute.snapshot.params.id )
        .subscribe( console.log );
    }

    ngOnInit(): void {
    }

}
