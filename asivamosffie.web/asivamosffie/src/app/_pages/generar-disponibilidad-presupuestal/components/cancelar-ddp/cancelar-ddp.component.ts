import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cancelar-ddp',
  templateUrl: './cancelar-ddp.component.html',
  styleUrls: ['./cancelar-ddp.component.scss']
})
export class CancelarDdpComponent implements OnInit {

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
  tipo: any;
  nSolicitud: any;

  constructor(public dialog: MatDialog,private disponibilidadServices: DisponibilidadPresupuestalService,private router: Router) {
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

      this.router.navigate(['/generarDisponibilidadPresupuestal']);
      dialogRef.close();
      this.dialog.closeAll();
  });
  }

  devolverSolicitud() {
    console.log(this.observaciones.value);
    let DisponibilidadPresupuestalObservacion={DisponibilidadPresupuestalId:this.id,Observacion:this.observaciones.value};
    this.disponibilidadServices.SetCancelDDP(DisponibilidadPresupuestalObservacion).subscribe(listas => {
      console.log(listas);
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
    });
  }

}
