import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
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
  esNovedad: boolean = false;
  novedadId: any;

  constructor(public dialog: MatDialog,  private router: Router,
    @Inject(MAT_DIALOG_DATA) public data,private disponibilidadServices: DisponibilidadPresupuestalService,
    private activatedRoute: ActivatedRoute
    ) {
    this.declararOnservaciones();
    this.minDate = new Date();
  }

  ngOnInit(): void {
    console.log(this.data);
    this.solicitudID=this.data.solicitudID;
    this.tipo=this.data.tipo;
    this.tipoSolicitud=this.data.tipoSolicitud;
    this.numeroSolicitud=this.data.numeroSolicitud;
    this.esNovedad = this.data.esNovedad;
    this.novedadId = this.data.novedadId;

    this.activatedRoute.params.subscribe(param => {
      console.log(param);
      //this.cargarServicio1(param.idTipoSolicitud);
    });

  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
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
        dialogRef.close();
        this.dialog.closeAll();
    });
  }

  devolverSolicitud() {
    let DisponibilidadPresupuestalObservacion={DisponibilidadPresupuestalId:this.solicitudID,Observacion:this.observaciones.value,NovedadContractualRegistroPresupuestalId:null};
    if(this.esNovedad){
      DisponibilidadPresupuestalObservacion.NovedadContractualRegistroPresupuestalId = this.novedadId;
    }
    if(this.tipo==0)
    {
       this.disponibilidadServices.SetReturnValidacionDDP(DisponibilidadPresupuestalObservacion, this.esNovedad, 0).subscribe(listas => {
         this.openDialog('', '<b>La solicitud ha sido devuelta al responsable técnico.</b>');
      });
    }
    else
    {
      this.disponibilidadServices.SetRechazarValidacionDDP(DisponibilidadPresupuestalObservacion, this.esNovedad).subscribe(listas => {
      this.openDialog('', '<b>La solicitud ha sido rechazada.</b>');

      });
    }


  }
}
