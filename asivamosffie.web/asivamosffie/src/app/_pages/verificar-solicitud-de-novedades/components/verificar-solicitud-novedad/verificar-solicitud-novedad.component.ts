import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import * as moment from 'moment';

@Component({
  selector: 'app-verificar-solicitud-novedad',
  templateUrl: './verificar-solicitud-novedad.component.html',
  styleUrls: ['./verificar-solicitud-novedad.component.scss']
})
export class VerificarSolicitudNovedadComponent implements OnInit {

  estaEditando = false;
  novedad: NovedadContractual;
  detalleId: number;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

  presupuestoModificado: number;
  plazoDiasModificado: number;
  plazoMesesModificado: number;
  validaParaModificar = false;

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required]
  });

  listaObservaciones = [
    {
      fecha: '25/10/2020',
      responsable: 'Supervisor',
      historial: 'Los documentos de soporte no cumplen con los requerimientos solicitados para registrar una novedad.'
    },
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'Es necesario revisar a detalle los soportes entregados, por lo tanto, hay que evaluar si en efecto no hay observaciones a la solicitud de novedad.'
    }
  ]

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      //console.log(this.detalleId);

      this.contractualNoveltyService.getNovedadContractualById( this.detalleId )
        .subscribe( respuesta => {
          this.novedad = respuesta;
            if(this.novedad?.estadoCodigo != "12" && this.novedad?.estadoCodigo != "19" && this.novedad?.estadoCodigo != "25" && this.novedad?.estadoCodigo != "26" && this.novedad?.estadoCodigo != null && this.novedad?.estadoCodigo != "26"  )
              this.validaParaModificar = true;

            this.addressForm.get('observaciones').setValue(this.novedad.observacionApoyo ? this.novedad.observacionApoyo.observaciones : null);
            this.addressForm.get('tieneObservaciones').setValue(this.novedad.tieneObservacionesApoyo);

            this.fechaFinalizacionContrato = this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.fechaFinContrato;

            //si el estado es en proceso y no debe quitarse aun hay que quitar esto, falta por definir
            this.fechaEstimadaFinalizacion = this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.fechaEstimadaFinProyecto;
            this.presupuestoModificado =  this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.valorTotalProyecto;
            this.plazoDiasModificado =  this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.plazoEstimadoDiasProyecto;
            this.plazoMesesModificado = (this.novedad?.datosContratoProyectoModificadosXNovedad[0]?.plazoEstimadoMesesProyecto * 30);

        });

    });
  }


  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    let novedad: NovedadContractual = {
      novedadContractualId: this.detalleId,
      tieneObservacionesApoyo: this.addressForm.value.tieneObservaciones,

      novedadContractualObservaciones: [
        {
          novedadContractualObservacionesId: this.novedad.observacionApoyo ? this.novedad.observacionApoyo.novedadContractualObservacionesId : 0,
          novedadContractualId: this.detalleId,
          esSupervision: false,
          esTramiteNovedades: null,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    }

    this.contractualNoveltyService.createEditObservacion(novedad, false)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
      });
  }

  updateFechaEstimada(pFechaFin: any, PffechaInicio: any){
    const fechaInicio = moment( new Date( PffechaInicio ).setHours( 0, 0, 0, 0 ) );
    const fechaFin = moment( new Date( pFechaFin ).setHours( 0, 0, 0, 0 ) );
    const duracionDias = fechaFin.diff( fechaInicio, 'days' );
    let rFecha;

    if(this.validaParaModificar == true){
      rFecha = moment(this.fechaEstimadaFinalizacion).add(duracionDias, 'days').toDate();
    }else{
      rFecha = this.fechaEstimadaFinalizacion;
    }
    return rFecha;
  }

  valuePresupuesto(presupuestoAdicional: number){
    return this.presupuestoModificado + (this.validaParaModificar == true ? (presupuestoAdicional > 0 ? presupuestoAdicional : 0) : 0);
  }

  valuePlazoProyecto(meses: number, dias: number){
    let diasAdicionales = this.plazoDiasModificado + (this.validaParaModificar == true ? (dias > 0 ? dias : 0) : 0);
    let mesesAdicionales = this.plazoMesesModificado + (this.validaParaModificar == true ? (meses > 0 ? meses * 30 : 0) : 0);

    let pDias = Math.trunc((diasAdicionales + mesesAdicionales)%30);
    let pMeses = Math.trunc((diasAdicionales + mesesAdicionales)/30);

    return pMeses + " Meses / " + pDias + " Días";
  }

}


