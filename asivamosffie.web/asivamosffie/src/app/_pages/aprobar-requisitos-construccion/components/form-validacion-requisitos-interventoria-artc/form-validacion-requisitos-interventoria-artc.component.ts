import { Contrato } from './../../../../_interfaces/faseUnoPreconstruccion.interface';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-validacion-requisitos-interventoria-artc',
  templateUrl: './form-validacion-requisitos-interventoria-artc.component.html',
  styleUrls: ['./form-validacion-requisitos-interventoria-artc.component.scss']
})
export class FormValidacionRequisitosInterventoriaArtcComponent implements OnInit {

  contrato: Contrato;
  perfilesCv: Dominio[] = [];

  constructor ( private faseUnoConstruccionService: FaseUnoConstruccionService,
                private commonSvc: CommonService,
                private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  };

  getContrato () {
    this.commonSvc.listaPerfil()
      .subscribe(
        perfiles => {
          this.perfilesCv = perfiles;
          this.faseUnoConstruccionService.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe( response => {
              this.contrato = response;
              console.log( this.contrato );
            } );
        }
      )
  };

  getTipoPerfil ( perfilCodigo: string ) {
    if ( this.perfilesCv.length > 0 ) {
      const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfilCodigo );
      return tipoPerfil[0].nombre;
    };
  };

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    };
  };

};