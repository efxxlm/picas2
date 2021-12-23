import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-avance-fisico-financiero',
  templateUrl: './avance-fisico-financiero.component.html',
  styleUrls: ['./avance-fisico-financiero.component.scss']
})
export class AvanceFisicoFinancieroComponent implements OnInit {
  @Input() esVerDetalle = false;
  @Input() seguimientoSemanal: any;
  @Input() avanceFisicoObs: string;
  @Input() pContratacionProyectoId: any;
  @Output() estadoSemaforoAvanceFisico = new EventEmitter<string>();
  semaforoAvanceFisico = 'sin-diligenciar';
  sinRegistros = false;

  constructor(private commonService: CommonService, private dialog: MatDialog) {}

  ngOnInit(): void {
    if (this.seguimientoSemanal !== undefined) {
      if (this.seguimientoSemanal.seguimientoSemanalAvanceFisico.length > 0) {
        console.log(this.seguimientoSemanal);
        const avanceFisico = this.seguimientoSemanal.seguimientoSemanalAvanceFisico[0];
        if (avanceFisico.registroCompletoHijo === false) {
          this.semaforoAvanceFisico = 'en-proceso';
        }
        if (avanceFisico.registroCompletoHijo === true) {
          this.semaforoAvanceFisico = 'completo';
        }
      }
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  descargarProject() {
    this.commonService
      .getDocumento(this.seguimientoSemanal.suportProyectRuta)
      .subscribe(
        response => {
          const documento = `support project`;
          const text = documento,
            blob = new Blob([response], { type: 'application/vnd.ms-project' }),
            anchor = document.createElement('a');
          anchor.download = documento;
          anchor.href = window.URL.createObjectURL(blob);
          anchor.dataset.downloadurl = [
            'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
            anchor.download,
            anchor.href
          ].join(':');
          anchor.click();
        },
        () => this.openDialog('', `<b>Archivo no encontrado.</b>`)
      );
  }
}
