import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';

@Component({
  selector: 'app-devolver-por-validacion',
  templateUrl: './devolver-por-validacion.component.html',
  styleUrls: ['./devolver-por-validacion.component.scss']
})
export class DevolverPorValidacionComponent implements OnInit {

  observaciones: FormControl;
  minDate: Date;

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
  id: any;

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService) {
    this.declararOnservaciones();
    this.minDate = new Date();
  }

  ngOnInit(): void {
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  private declararOnservaciones() {
    this.observaciones = new FormControl(null, [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  devolverSolicitud() {
    console.log(this.observaciones.value);
    let DisponibilidadPresupuestalObservacion={DisponibilidadPresupuestalId:this.id,Observacion:this.observaciones.value};
    this.disponibilidadServices.SetReturnDDP(DisponibilidadPresupuestalObservacion).subscribe(listas => {
      console.log(listas);
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
    });

  }

}
