import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-manejo-anticipo-artc',
  templateUrl: './manejo-anticipo-artc.component.html',
  styleUrls: ['./manejo-anticipo-artc.component.scss']
})
export class ManejoAnticipoArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionObservacionId: [],

  });

  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  observacionManejoAnticipo = '3';
  totalGuardados = 0;
  dataTablaHistorialObservacion: any[] = [];
  dataTablaHistorialApoyo: any[] = [];
  dataSource = new MatTableDataSource();
  dataSourceApoyo = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaRevision',
    'observacionesSupervision'
  ];
  @Input() observacionesCompleted;
  @Input() contratacion: any;
  @Input() contratoConstruccionId: any;
  @Output() createEdit = new EventEmitter();

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  { }

  ngOnInit(): void {
    if (this.contratacion) {
      this.addressForm.get('tieneObservaciones')
        .setValue( this.contratacion.tieneObservacionesManejoAnticipoSupervisor !== undefined ?
          this.contratacion.tieneObservacionesManejoAnticipoSupervisor : null );
      this.addressForm.get('observaciones')
        .setValue( this.contratacion.observacionManejoAnticipoSupervisor !== undefined ?
          this.contratacion.observacionManejoAnticipoSupervisor.observaciones : null );
      this.addressForm.get('construccionObservacionId')
        .setValue(this.contratacion.observacionManejoAnticipoSupervisor !== undefined ?
          this.contratacion.observacionManejoAnticipoSupervisor.construccionObservacionId : null );
    }
    this.getDataTable();
  }

  getDataTable() {
    this.contratacion.construccionObservacion.forEach( observacion => {
      if (  observacion.tipoObservacionConstruccion === this.observacionManejoAnticipo
            && observacion.observaciones !== undefined
            && observacion.esSupervision === true ) {
        this.dataTablaHistorialObservacion.push( observacion );
      }
      if (  observacion.tipoObservacionConstruccion === this.observacionManejoAnticipo
            && observacion.observaciones !== undefined
            && observacion.esSupervision === false ) {
        this.dataTablaHistorialApoyo.push( observacion );
      }
    } );
    if ( this.dataTablaHistorialObservacion.length > 0 ) {
      this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
    }
    if ( this.dataTablaHistorialApoyo.length > 0 ) {
      this.dataSourceApoyo = new MatTableDataSource( this.dataTablaHistorialApoyo );
    }
  }

  validarSemaforo() {

    this.contratacion.semaforoManejo = 'sin-diligenciar';

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.contratacion.semaforoManejo = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones) {
        this.contratacion.semaforoManejo = 'en-proceso';
      }
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto !== undefined ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length > 1000 ? 1000 : textolimpio.length;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  guardarManejo() {

    const construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesManejoAnticipoSupervisor: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.ManejoAnticipo,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones,

        }
      ]
    };

    console.log( construccion );

    if (  this.addressForm.value.tieneObservaciones === false
          && this.totalGuardados === 0
          && this.contratacion.tieneObservacionesManejoAnticipoApoyo === true ) {
      this.openDialog( '', '<b>Le recomendamos verificar su respuesta; tenga en cuenta que el apoyo a la supervisión si tuvo observaciones.</b>' );
      this.totalGuardados++;
      return;
    }
    if ( this.totalGuardados === 1 && this.addressForm.value.tieneObservaciones !== null ) {
      this.faseDosAprobarConstruccionSvc.createEditObservacionManejoAnticipoSupervisor( construccion )
        .subscribe(
          response => this.openDialog( '', response.message ),
          err => this.openDialog( '', err.message )
        );
    }

  }

}
