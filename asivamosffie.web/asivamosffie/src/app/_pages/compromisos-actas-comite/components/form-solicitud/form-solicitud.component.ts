import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { CommonService } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-form-solicitud',
  templateUrl: './form-solicitud.component.html',
  styleUrls: ['./form-solicitud.component.scss']
})
export class FormSolicitudComponent implements OnInit {

  @Input() solicitudes: any;
  @Input() esComiteFiduciario: boolean = false;
  totalAprobado: number = 0;
  totalNoAprobado: number = 0;
  resultadoVotacion: string;
  estadoSolicitud: any[] = [];
  addressForm = this.fb.group({
    estadoSolicitud   : [null, Validators.required],
    observaciones     : [null, Validators.required],
    url               : null,
    tieneCompromisos  : [null, Validators.required],
    cuantosCompromisos: [null, Validators.required],
    compromisos       : this.fb.array([])
  });
  estadosArray = [
    { 
      name: 'Devuelto por comité', 
      value: 'devueltoComite' 
    }
  ];
  tipoSolicitud: any[] = [];
  tipoSolicitudCodigo = {
    procesoSeleccion: '1',
    contratacion: '2',
    modificacionContractual: '3',
    controversiaContractual: '4',
    defensaJudicial: '5'
  }

  get compromisos() {
    return this.addressForm.get('compromisos') as FormArray;
  };

  constructor ( private fb       : FormBuilder,
                private commonSvc: CommonService ) 
  {
  };

  ngOnInit(): void {
    this.resultadosVotaciones( this.solicitudes )
    this.commonSvc.listaEstadoSolicitud()
    .subscribe( ( resp: any[] ) => {
      this.estadoSolicitud = resp.filter( estado => this.solicitudes.estadoCodigo === estado.codigo );
    } );
  };

  innerObservacion ( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return `<b>${ observacionHtml }</b>`;
    };
  };

  resultadosVotaciones ( solicitud: any ) {
    solicitud.sesionSolicitudVoto.forEach( sv => {
      if (sv.esAprobado)
        this.totalAprobado++;
      else
        this.totalNoAprobado++;
    })

    if ( this.totalNoAprobado > 0 )
      this.resultadoVotacion = 'No Aprobó'
    else
      this.resultadoVotacion = 'Aprobó'
  };

  compromisosFiduciario ( compromisos: any ) {
    const compromisosCF = [];
    compromisos.forEach( compromiso => {
      if ( compromiso.esFiduciario === true ) {
        compromisosCF.push( compromiso );
      };
    } );
    return compromisosCF;
  }

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

}
