import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-diagnostico-artc',
  templateUrl: './diagnostico-artc.component.html',
  styleUrls: ['./diagnostico-artc.component.scss']
})
export class DiagnosticoArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null],
    construccionObservacionId: []
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
  @Input() observacionesCompleted: boolean;
  @Input() construccion: any;
  @Input() contratoConstruccionId: any;

  @Output() createEditDiagnostico = new EventEmitter();
  @Output() estadoSemaforoDiagnostico = new EventEmitter<string>();

  dataTablaHistorialObservacion: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'fechaRevision',
    'observacionesSupervision'
  ];
  constructor ( private dialog: MatDialog, 
                private fb: FormBuilder,
                private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  {
    this.getDataPlanesProgramas();
  };

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
    if (this.construccion) {
      this.addressForm.get('tieneObservaciones').setValue( this.construccion.tieneObservacionesDiagnosticoSupervisor !== undefined ? this.construccion.tieneObservacionesDiagnosticoSupervisor : null )
      this.addressForm.get('observaciones').setValue( this.construccion.observacionDiagnosticoSupervisor !== undefined ? this.construccion.observacionDiagnosticoSupervisor.observaciones : null )
      this.addressForm.get('construccionObservacionId').setValue(this.construccion.observacionDiagnosticoSupervisor !== undefined ? this.construccion.observacionDiagnosticoSupervisor.construccionObservacionId : null);
    };
  };

  getDataPlanesProgramas () {
    this.dataTablaHistorialObservacion.push(
      {
        fechaRevision:'10/08/2020',
        observacionesSupervision: 'El valor del costo directo debe ser de $20.000.000 y la utilidad de $14.000.000, realice el ajuste y tenga en cuenta que estos valores deben ser corregidos para poder continuar.'
      }
    );
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  textoLimpio(texto: string) {
    if ( texto !== undefined ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  guardarDiagnostico() {

    let construccion = {
      contratoConstruccionId: this.contratoConstruccionId,
      tieneObservacionesDiagnosticoSupervisor: this.addressForm.value.tieneObservaciones,
      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.Diagnostico,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    };

    console.log(construccion);
    this.faseDosAprobarConstruccionSvc.createEditObservacionDiagnosticoSupervisor( construccion )
      .subscribe(
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      );

  };

}
