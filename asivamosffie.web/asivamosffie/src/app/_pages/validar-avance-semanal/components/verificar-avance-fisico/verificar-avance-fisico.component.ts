import { Component, Input, OnInit } from '@angular/core';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-verificar-avance-fisico',
  templateUrl: './verificar-avance-fisico.component.html',
  styleUrls: ['./verificar-avance-fisico.component.scss']
})
export class VerificarAvanceFisicoComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Input() avanceFisicoObs: string;
    @Input() estadoSemaforo: string;
    semaforoAvanceFisico = 'sin-diligenciar';

    constructor(private commonService: CommonService, private dialog: MatDialog) { }

    ngOnInit(): void {
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
