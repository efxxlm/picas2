import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-formulario-tiene-observaciones',
  templateUrl: './formulario-tiene-observaciones.component.html',
  styleUrls: ['./formulario-tiene-observaciones.component.scss']
})
export class FormularioTieneObservacionesComponent implements OnInit {
  addressForm = this.fb.group({});
  constructor(private router: Router,public dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });   
  }
  editorStyle = {
    height: '100px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: ['', Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  generarActaSuscrita(){
    alert("llama al servicio");
  }
  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('La información ha sido guardada exitosamente.', "");
    this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
  }
}
