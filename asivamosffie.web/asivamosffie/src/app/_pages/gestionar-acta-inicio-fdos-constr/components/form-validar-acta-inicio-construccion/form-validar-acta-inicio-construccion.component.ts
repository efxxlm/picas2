import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-validar-acta-inicio-construccion',
  templateUrl: './form-validar-acta-inicio-construccion.component.html',
  styleUrls: ['./form-validar-acta-inicio-construccion.component.scss']
})
export class FormValidarActaInicioConstruccionComponent implements OnInit {
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  public editable: boolean;
  public title;
  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder) { }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
    if(localStorage.getItem("editable")=="true"){
      this.editable=true;
      this.title='Ver detalle/Editar';
    }
    else{
      this.editable=false;
      this.title='Validar';
    }
  }
  loadData(id) {
    if(this.editable==true){
      console.log("cargar servicio");
    }
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
    this.router.navigate(['/generarActaInicioConstruccion']);
  }

}
