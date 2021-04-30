import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual, NovedadContractualObservaciones } from 'src/app/_interfaces/novedadContractual';
@Component({
  selector: 'app-registrar-revision-novedad',
  templateUrl: './registrar-revision-novedad.component.html',
  styleUrls: ['./registrar-revision-novedad.component.scss']
})
export class RegistrarRevisionNovedadComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    fechaEnvioGestionContractual: [null, Validators.required],
    estadoProcesoCodigo: [null, Validators.required],
    fechaAprobacionGestionContractual: [null, Validators.required],
    abogadoRevisionId: [null, Validators.required],
    observacionDevolver: [],
    novedadContractualObservacionesId: [],
  });

  estaEditando = false;

  estadoDelProcesoArray = [];
  nombreAbogadoPresentoSolicitudArray = [];

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  @Input() novedad: NovedadContractual;
  @Output() guardar = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private commonService: CommonService,

  ) {
    commonService.listaEstadoProcesoNovedades()
      .subscribe(respuesta => {
        this.estadoDelProcesoArray = respuesta;
      });

    commonService.listaAbogadoRevisionNovedades()
      .subscribe(respuesta => {
        this.nombreAbogadoPresentoSolicitudArray = respuesta;
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.novedad) {
      this.addressForm.get('fechaEnvioGestionContractual').setValue(this.novedad.fechaEnvioGestionContractual)
      this.addressForm.get('estadoProcesoCodigo').setValue(this.novedad.estadoProcesoCodigo)
      this.addressForm.get('fechaAprobacionGestionContractual').setValue(this.novedad.fechaAprobacionGestionContractual)
      this.addressForm.get('abogadoRevisionId').setValue(this.novedad.abogadoRevisionId?.toString())

      console.log(this.novedad.observacionTramite)
      if (this.novedad.observacionTramite) {
        
        this.addressForm.get('novedadContractualObservacionesId').setValue(this.novedad.observacionTramite.novedadContractualObservacionesId);
        this.addressForm.get('observacionDevolver').setValue(this.novedad.observacionTramite.observaciones);
      }
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
    }
  }

  ngOnInit(): void {
  }

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ],
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {

    let contadorConcurrencias = 0;
    let posicion = 0;
    if (cadena) {
      while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
        ++contadorConcurrencias;
        posicion += subcadena.length;
      }
    }
    return contadorConcurrencias;
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {

    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    this.novedad.fechaEnvioGestionContractual = this.addressForm.get('fechaEnvioGestionContractual').value;
    this.novedad.estadoProcesoCodigo = this.addressForm.get('estadoProcesoCodigo').value;
    this.novedad.fechaAprobacionGestionContractual = this.addressForm.get('fechaAprobacionGestionContractual').value;
    this.novedad.abogadoRevisionId = this.addressForm.get('abogadoRevisionId').value;

    //if ( this.novedad.estadoProcesoCodigo === '3' )
    {

      let novedadContractualObservaciones: NovedadContractualObservaciones = {
        esTramiteNovedades: true,
        novedadContractualId: this.novedad.novedadContractualId,
        novedadContractualObservacionesId: this.addressForm.get('novedadContractualObservacionesId').value,
        observaciones: this.addressForm.get('observacionDevolver').value
      }

      this.novedad.novedadContractualObservaciones = [],
        this.novedad.novedadContractualObservaciones.push(novedadContractualObservaciones);
    }



    this.guardar.emit(true);

  }
}
