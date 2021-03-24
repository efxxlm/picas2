import { Router, ActivatedRoute } from '@angular/router';
import { Dominio, CommonService } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';

@Component({
  selector: 'app-obs-cargar-forma-pago',
  templateUrl: './obs-cargar-forma-pago.component.html',
  styleUrls: ['./obs-cargar-forma-pago.component.scss']
})
export class ObsCargarFormaPagoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() cargarFormaPagoCodigo: string;
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() tieneFormaPago: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoObservacionId = 0;
    addressForm: FormGroup;
    formaPagoArray: Dominio[] = [];
    editorStyle = {
      height: '45px',
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
    estaEditando = false;
    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private activatedRoute: ActivatedRoute,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private commonSvc: CommonService )
    {
        this.addressForm = this.crearFormulario();
        this.commonSvc.formasDePago()
            .subscribe( response => this.formaPagoArray = response );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group({
            fechaCreacion: [ null ],
            tieneObservaciones: [null, Validators.required],
            observaciones:[null, Validators.required],
        })
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getFormaPago( formaPagoCodigo: string ) {
        if ( this.formaPagoArray.length > 0 && formaPagoCodigo !== undefined ) {
            const forma = this.formaPagoArray.filter( forma => forma.codigo === formaPagoCodigo );
            if ( forma.length > 0 ) {
                return forma[0].nombre;
            }
        }
    }

}
