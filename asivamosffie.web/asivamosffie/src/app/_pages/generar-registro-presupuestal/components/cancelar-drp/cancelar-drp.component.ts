import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-cancelar-drp',
  templateUrl: './cancelar-drp.component.html',
  styleUrls: ['./cancelar-drp.component.scss']
})
export class CancelarDrpComponent implements OnInit {
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  id: any;
  tipo: any;
  fecha:any;
  nSolicitud: any;
  estaEditando = false;
  constructor(public dialog: MatDialog,private fb: FormBuilder,private router: Router,
     private disponibilidadServices:DisponibilidadPresupuestalService) { }

  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
  }
  openDialog(modalTitle: string, modalText: string,relocate=false) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
    if(relocate)
      {
        dialogRef.afterClosed().subscribe(result => {
          this.router.navigate(["/generarRegistroPresupuestal"], {});
         });
      }
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
  crearFormulario(){
    return this.fb.group({
      objeto: [null, Validators.required]
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio ( texto: string ) {
    if ( texto ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  };

  onSubmit() {
    this.estaEditando = true;
    let DisponibilidadPresupuestalObservacion={DisponibilidadPresupuestalId:this.id,Observacion:this.addressForm.value.objeto};
    this.disponibilidadServices.SetCancelDDR(DisponibilidadPresupuestalObservacion).subscribe(listas => {
      console.log(listas);
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>',true);
    });
  }
}
