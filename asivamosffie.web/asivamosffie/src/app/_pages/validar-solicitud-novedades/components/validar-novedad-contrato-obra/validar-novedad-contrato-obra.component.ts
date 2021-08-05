import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import * as moment from 'moment';

@Component({
  selector: 'app-validar-novedad-contrato-obra',
  templateUrl: './validar-novedad-contrato-obra.component.html',
  styleUrls: ['./validar-novedad-contrato-obra.component.scss']
})
export class ValidarNovedadContratoObraComponent implements OnInit {
  detalleId: number;
  estaEditando = false;
  novedad: NovedadContractual;
  fechaFinalizacionContrato : any;
  fechaEstimadaFinalizacion : Date;

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required]
  });

  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }]
    ]
  };

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
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
    private contractualNoveltyService: ContractualNoveltyService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      //console.log(this.detalleId);

      this.contractualNoveltyService.getNovedadContractualById(this.detalleId).subscribe(respuesta => {
        this.novedad = respuesta;
        if(this.novedad) this.estaEditando = true;

        let observacion = this.novedad.observacionApoyo ? this.novedad.observacionApoyo.observaciones : '';

        this.addressForm
          .get('observaciones')
          .setValue(
            this.novedad.observacionSupervisor ? this.novedad.observacionSupervisor.observaciones : observacion
          );
        this.addressForm.get('tieneObservaciones').setValue(this.novedad.tieneObservacionesSupervisor);

        this.fechaFinalizacionContrato = (this.novedad?.contrato?.fechaTerminacionFase2 ? this.novedad?.contrato?.fechaTerminacionFase2 : this.novedad?.contrato?.fechaTerminacion);
        this.fechaFinalizacionContrato = moment( new Date( this.fechaFinalizacionContrato ).setHours( 0, 0, 0, 0 ) );
        respuesta.novedadContractualDescripcion.forEach( d => {
          const fechaInicio = moment( new Date( d?.fechaInicioSuspension ).setHours( 0, 0, 0, 0 ) );
          const fechaFin = moment( new Date( d?.fechaFinSuspension ).setHours( 0, 0, 0, 0 ) );
          const duracionDias = fechaFin.diff( fechaInicio, 'days' );
          d.fechaEstimadaFinalizacion = moment(this.fechaFinalizacionContrato).add(duracionDias, 'days').toDate();
        });

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
    // console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    let novedad: NovedadContractual = {
      novedadContractualId: this.detalleId,
      tieneObservacionesSupervisor: this.addressForm.value.tieneObservaciones,

      novedadContractualObservaciones: [
        {
          novedadContractualObservacionesId: this.novedad.observacionSupervisor
            ? this.novedad.observacionSupervisor.novedadContractualObservacionesId
            : 0,
          novedadContractualId: this.detalleId,
          esSupervision: true,
          esTramiteNovedades: null,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    };

    this.contractualNoveltyService.createEditObservacion(novedad, true).subscribe(respuesta => {
      this.openDialog('', respuesta.message);
      if (respuesta.code == '200') {
        this.router.navigate(['/validarSolicitudDeNovedades']);
      }
    });
  }
}
