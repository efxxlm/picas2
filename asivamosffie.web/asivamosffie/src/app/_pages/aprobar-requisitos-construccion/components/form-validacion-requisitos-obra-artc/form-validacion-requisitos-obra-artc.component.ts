import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-validacion-requisitos-obra-artc',
  templateUrl: './form-validacion-requisitos-obra-artc.component.html',
  styleUrls: ['./form-validacion-requisitos-obra-artc.component.scss']
})
export class FormValidacionRequisitosObraArtcComponent implements OnInit {

  contrato: Contrato;

  constructor ( private faseDosConstruccionSvc: FaseUnoConstruccionService,
                private activatedRoute: ActivatedRoute )
  {
    this.getContrato();
  }

  ngOnInit(): void {
  };

  getContrato () {
    this.faseDosConstruccionSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
      .subscribe( response => {
        this.contrato = response;
        console.log( this.contrato );
      } );
  };

  Cargar ( seGuardo: boolean ) {
    if ( seGuardo ) {
      this.contrato = null;
      this.getContrato();
    };
  };

};