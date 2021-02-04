import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-obs-registrar-sol-pago-autoriz',
  templateUrl: './obs-registrar-sol-pago-autoriz.component.html',
  styleUrls: ['./obs-registrar-sol-pago-autoriz.component.scss']
})
export class ObsRegistrarSolPagoAutorizComponent implements OnInit {

    @Input() contrato: any;
    @Input() esVerDetalle = false;
    solicitudPago: any;
    solicitudPagoFase: any;
    addressForm: FormGroup;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'faseContrato',
      'pagosRealizados',
      'valorFacturado',
      'porcentajeFacturado',
      'saldoPorPagar',
      'porcentajePorPagar'
    ];
    dataTable: any[] = [
      {
        faseContrato: 'Fase 1 - Preconstrucción',
        pagosRealizados: '0',
        valorFacturado: '0',
        porcentajeFacturado: '0',
        saldoPorPagar: '$30.000.000',
        porcentajePorPagar: '100%',
      },
      {
        faseContrato: 'Fase 2 - Construcción',
        pagosRealizados: '0',
        valorFacturado: '0',
        porcentajeFacturado: '0',
        saldoPorPagar: '$75.000.000',
        porcentajePorPagar: '100%',
      }
    ];
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

    constructor(
        private fb: FormBuilder,
        private dialog: MatDialog )
    {
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {
            this.solicitudPago = this.contrato.solicitudPagoOnly;
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        }

        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    crearFormulario() {
        return this.fb.group({
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

    onSubmit() {
      console.log(this.addressForm.value);
    }

}
