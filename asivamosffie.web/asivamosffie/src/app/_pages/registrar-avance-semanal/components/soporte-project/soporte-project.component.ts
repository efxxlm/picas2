import { Component, Input, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { RegistrarAvanceSemanalService } from 'src/app/core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-soporte-project',
  templateUrl: './soporte-project.component.html',
  styleUrls: ['./soporte-project.component.scss']
})
export class SoporteProjectComponent implements OnInit {
  @Input() pContratacionProyectoId: any;
  @Input() seguimientoSemanal: any;

  archivo: string;

  documentFile: FormControl;
  file: any;

  constructor(private avanceSemanalSvc: RegistrarAvanceSemanalService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.declararDocumentFile();
    if (this.seguimientoSemanal.infoProyecto.fechaUltimoReporte.contratacionProyecto.suportProyectRuta)
      this.archivo =
        this.seguimientoSemanal.infoProyecto.fechaUltimoReporte.contratacionProyecto.suportProyectRuta.split(
          `SeguimientoSemanalAvanceFisico\\${this.pContratacionProyectoId}\\`
        )[1];
  }

  private declararDocumentFile() {
    this.documentFile = new FormControl(null, [Validators.required]);
  }

  fileName(event: any) {
    if (event.target.files.length > 0) {
      this.file = event.target.files[0];
      this.archivo = event.target.files[0].name;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  guardar() {
    console.log('file: ', this.file);

    this.avanceSemanalSvc.UploadFileSeguimientoSemanalAvanceFisico(this.pContratacionProyectoId, this.file).subscribe(
      async response => {
        this.openDialog('', `<b>${response.message}</b>`);
      },
      err => this.openDialog('', `<b>${err.message}</b>`)
    );
  }
}
