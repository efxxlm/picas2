import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-diagnostico-artc',
  templateUrl: './diagnostico-artc.component.html',
  styleUrls: ['./diagnostico-artc.component.scss']
})
export class DiagnosticoArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
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
  @Input() observacionesCompleted;

  dataTablaHistorialObservacion: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [ 
    'fechaRevision',
    'observacionesSupervision'
  ];
  constructor(private dialog: MatDialog, private fb: FormBuilder) {
    this.getDataPlanesProgramas ();
   }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.dataTablaHistorialObservacion );
  }
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
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }
  
  openDialog (modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  };

  onSubmit(){
    this.openDialog( 'La información ha sido guardada exitosamente.', '' );
  }
}
