import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TiposObservacionConstruccion } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-diagnostico-artc',
  templateUrl: './diagnostico-artc.component.html',
  styleUrls: ['./diagnostico-artc.component.scss']
})
export class DiagnosticoArtcComponent implements OnInit, OnChanges {

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

  dataTablaHistorialObservacion: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'fechaRevision',
    'observacionesSupervision'
  ];
  constructor(private dialog: MatDialog, private fb: FormBuilder) {
    this.getDataPlanesProgramas ();
  };

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.contratacion){
      this.ngOnInit();
      console.log("c", this.construccion);
    };
  };

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
    if (this.construccion) {
      this.addressForm.get('tieneObservaciones') // Por integrar
      this.addressForm.get('observaciones') //Por integrar
      this.addressForm.get('construccionObservacionId').setValue(this.construccion.observacionDiagnostico ? this.construccion.observacionDiagnostico.construccionObservacionId : null);

      this.validarSemaforo();
    };
  };

  validarSemaforo() {

    this.construccion.semaforoDiagnostico = "sin-diligenciar";

    if (this.addressForm.value.tieneObservaciones === true || this.addressForm.value.tieneObservaciones === false) {
      this.construccion.semaforoDiagnostico = 'completo';

      if (this.addressForm.value.tieneObservaciones === true && !this.addressForm.value.observaciones)
        this.construccion.semaforoDiagnostico = 'en-proceso';
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
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
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
      tieneObservacionesDiagnosticoApoyo: this.addressForm.value.tieneObservaciones,

      construccionObservacion: [
        {
          construccionObservacionId: this.addressForm.value.construccionObservacionId,
          contratoConstruccionId: this.contratoConstruccionId,
          tipoObservacionConstruccion: TiposObservacionConstruccion.Diagnostico,
          esSupervision: true,
          esActa: false,
          observaciones: this.addressForm.value.observaciones,

        }
      ]
    };

    console.log(construccion);

  };

}
