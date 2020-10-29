import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
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
  solicitudID: any;
  tipo: any;
  tipoSolicitud: any;
  numeroSolicitud: any;

  constructor(public dialog: MatDialog,  private router: Router,
    @Inject(MAT_DIALOG_DATA) public data,private disponibilidadServices: DisponibilidadPresupuestalService) {
    this.declararOnservaciones();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    this.solicitudID=this.data.solicitudID;
    this.tipo=this.data.tipo;
    this.tipoSolicitud=this.data.tipoSolicitud;
    this.numeroSolicitud=this.data.numeroSolicitud;
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
    let dialogRef= this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {

        this.router.navigate(['/validarDisponibilidadPresupuesto']);

    });
  }

  devolverSolicitud() {
    //console.log(this.observaciones.value);
    let DisponibilidadPresupuestalObservacion={DisponibilidadPresupuestalId:this.solicitudID,Observacion:this.observaciones.value};
    if(this.tipo==0)
    {
      this.disponibilidadServices.SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion).subscribe(listas => {
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');

      });
    }
    else
    {
      this.disponibilidadServices.SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion).subscribe(listas => {
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');

      });
    }


  }
}
