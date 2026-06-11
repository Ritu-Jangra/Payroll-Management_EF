import { ReasonType } from '../Enums/ReasonsType';
import { StatusType } from '../Enums/StatusType';

export interface ILeave {
  leaveId: string;
  startDate: string;
  endDate: string;
  reason: ReasonType;
  status: StatusType;
  employeeName: string;
  approverName?: string;
}
