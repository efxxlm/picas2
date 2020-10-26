import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-programacion-obra-verificar-requisitos',
  templateUrl: './programacion-obra-verificar-requisitos.component.html',
  styleUrls: ['./programacion-obra-verificar-requisitos.component.scss']
})
export class ProgramacionObraVerificarRequisitosComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
  });

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

  @Input() observacionesCompleted;
  @Input() contratoConstruccion: any;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private commonService: CommonService,
    private faseUnoConstruccionService: FaseUnoConstruccionService
  ) { }

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  descargar() {
    this.commonService.getFileById(this.contratoConstruccion.archivoCargueIdProgramacionObra)
      .subscribe(respuesta => {
        let documento = "ProgramacionObra.xlsx";
        //console.log(documento);

        //console.log(result);
        /*var url = window.URL.createObjectURL(result);
        window.open(url);
        //console.log("download result ", result);*/
        var text = documento,
          blob = new Blob([respuesta], { type: 'application/octet-stream' }),
          anchor = document.createElement('a');
        anchor.download = documento;
        //anchor.href = (window.webkitURL || window.URL).createObjectURL(blob);
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        //console.log(anchor);
        anchor.click();
      });
  }

  onSubmit() {
    this.openDialog('La información ha sido guardada exitosamente.', '');
  }

}
